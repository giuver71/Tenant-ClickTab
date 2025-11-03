import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { HeaderButton } from 'src/app/elements/page-header/page-header.component';
import { RoleDTO } from 'src/app/models/generics/role.model';
import { RoleRuleDTO } from 'src/app/models/generics/rolerule.model';
import { RuleDTO } from 'src/app/models/generics/rule.model';
import { DialogService } from 'src/app/services/dialog.service';
import { RoleService } from 'src/app/services/generics/role.services';
import { RuleService } from 'src/app/services/generics/rule.services';
import { MessageBarService } from 'src/app/services/messagebar.service';

@Component({
  selector: 'app-add-roles',
  templateUrl: './add-roles.component.html',
  styleUrl: './add-roles.component.scss'
})
export class AddRolesComponent implements OnInit{
  role:RoleDTO=new RoleDTO();
  rules:Array<RuleDTO>=new Array<RuleDTO>();
  roleForm!: FormGroup;
  buttons: Array<HeaderButton> = new Array<HeaderButton>();
  columns:Array<ConfigColumn>=new Array<ConfigColumn>;
  loaded:boolean=false;
  constructor(
    
    private roleService:RoleService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private activateRoute: ActivatedRoute,
    private messageBar: MessageBarService,
    private router:Router,
    private ruleService:RuleService
  ) {
    
  }
  async ngOnInit() {
    await this.getRules();
    if( this.activateRoute.snapshot.params["id"]!=null){
          await this.getRole();
      }  
      else{
        this.role=new RoleDTO();
        // this.user.UserRoles=new Array<UserRoleDTO>();
    
        }
        this.configureHeaderButtons();
        this.createForm();
        this.configureColumns();
        this.applyRoleRule(this.role);
  }

   configureHeaderButtons() {
    this.buttons = [
      {
        label: 'Salva Ruolo',
        icon: 'fa fa-save',
        color: 'btn-secondary',
        isDisabled: true,
        action: () => this.save()
      },

    ];
  }

  createForm(){
     this.roleForm = this.formBuilder.group({
          description: [this.role?.Description, Validators.required],
        });
    
        this.roleForm.statusChanges.subscribe(() => {
          if (!this.roleForm.invalid) {
            this.buttons[0].isDisabled = false;
          }
        });
  }

  configureColumns(){
    this.columns=[
        { key: "RuleDescription", display: 'Regola',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
        { key: "IsChecked", display: "Associa", type: TypeColumn.Checkbox, styles: { flex: "0 0 40%" }},
      ];
  }

  async getRules(){
    await  this.ruleService.getAll().then((res)=>{
      this.rules=res;
    }).catch((err)=>{
      console.error("AddRole.getRules()", err);
      DialogService.Error(err.message);
    })
  }

  async getRole(){
      let id= this.activateRoute.snapshot.params["id"]
        await this.roleService.getFull(id).then((res) => {
          this.role = res;
          this.loaded=true;
        }).catch((err) => {
          console.error("AddRole.getRole()", err);
          DialogService.Error(err.message);
        });
  }

  applyRoleRule(role:RoleDTO){
      this.rules.forEach(r=>{
        var ur=this.role.RoleRules.find(p=>p.FK_Rule==r.ID);
        if(ur==null){
          let addRoleRule:RoleRuleDTO=new RoleRuleDTO();
          addRoleRule.FK_Rule=r.ID;
          addRoleRule.FK_Role=this.role.ID;
          addRoleRule.RuleDescription=r.Description;
          this.role.RoleRules.push(addRoleRule);
        }
        else{
          ur.IsChecked=true;
        }
      });
     this.loaded=true;
    }

  save(){
     this.role.RoleRules=this.role.RoleRules.filter(p=>p.IsChecked);
      this.roleService.saveRole(this.role).then((res) => {
     this.messageBar.show({
        message: 'Ruolo salvato correttamente!',
        type: 'success',
        duration: 4000,
        actionLabel: 'Chiudi',
        onClose: () => this.router.navigate(['/registry/list-roles'])
      });
    }).catch((err) => {
      console.error("addRole.Save", err);
      DialogService.Error(err.message);
    }) 
  }

}
