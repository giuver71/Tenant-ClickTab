import { Component, Input, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { UserDTO } from '../../../../models/generics/user.model';
import { UserService } from '../../../../services/user.service';
import { DialogService } from '../../../../services/dialog.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HeaderButton } from '../../../../elements/page-header/page-header.component';
import { RoleDTO } from '../../../../models/generics/role.model';
import { RoleService } from '../../../../services/generics/role.services';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRoleDTO } from '../../../../models/generics/userRole.model';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { MessageBarService } from '../../../../services/messagebar.service';
@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent implements OnInit {
  constructor(
    private userService: UserService,
    private roleService:RoleService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private activateRoute: ActivatedRoute,
    private messageBar: MessageBarService,
    private router:Router
  ) {


  }
  userForm!: FormGroup;
  buttons: Array<HeaderButton> = new Array<HeaderButton>();
  roles!: Array<RoleDTO>;
  selectedRoles!:Array<RoleDTO>;
  user!:UserDTO;
  columns:Array<ConfigColumn>=new Array<ConfigColumn>;
  loaded:boolean=false;
  @ViewChild('permissionTemplate', { static: true }) permissionTemplate: TemplateRef<any>;
  
  async ngOnInit() {
    await this.getRoles();
    if( this.activateRoute.snapshot.params["id"]!=null){
      await this.getUser();
    }  
    else{
      this.user=new UserDTO();
      this.user.UserRoles=new Array<UserRoleDTO>();

    }
    this.configureColumns();
    this.applyUserRole(this.user);
    

    this.createForm();
    this.configureHeaderButtons();
  }

  configureHeaderButtons() {
    this.buttons = [
      {
        label: 'Salva Utente',
        icon: 'fa fa-save',
        color: 'btn-secondary',
        isDisabled: true,
        action: () => this.save()
      },

    ];
  }

  configureColumns(){
    this.columns=[
      { key: "Role.Description", display: 'Ruolo',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
      { key: "IsChecked", display: "Associa", type: TypeColumn.Checkbox, styles: { flex: "0 0 40%" }},
    ];
  }

  createForm() {
    this.userForm = this.formBuilder.group({
      name: [this.user?.Name, Validators.required],
      surname: [this.user?.Surname, Validators.required],
      // dob: [ this.utilityService.parseDatefromDB(this.user.DOB) , Validators.required],
      email: [this.user?.Email, Validators.required],
    });

    this.userForm.statusChanges.subscribe(() => {
      if (!this.userForm.invalid) {
        this.buttons[0].isDisabled = false;
      }
    });
  }

  async getRoles(){
   await  this.roleService.getAll().then((res)=>{
      this.roles=res;
    }).catch((err)=>{
      console.error("AddUser.getRoles()", err);
      DialogService.Error(err.message);
    })
  }

  async getUser() {
    let id= this.activateRoute.snapshot.params["id"]
    await this.userService.getFull(id).then((res) => {
      this.user = res;
    }).catch((err) => {
      console.error("AddUser.getUser()", err);
      DialogService.Error(err.message);
    });

  }

  save() {
    this.user.UserRoles=this.user.UserRoles.filter(p=>p.IsChecked);
    this.userService.saveUser(this.user).then((res) => {
     this.messageBar.show({
        message: 'Utente salvato correttamente!',
        type: 'success',
        duration: 4000,
        actionLabel: 'Chiudi',
        onClose: () => this.router.navigate(['/registry/list-users'])
  });
    }).catch((err) => {
      console.error("addUser.Save", err);
      DialogService.Error(err.message);
    }) 
  }

  // Crea gli UserRole per l'utente corrente 
  applyUserRole(user:UserDTO){
    this.roles.forEach(r=>{
      var ur=this.user.UserRoles.find(p=>p.FK_Role==r.ID);
      if(ur==null){
        let addUserRole:UserRoleDTO=new UserRoleDTO();
        addUserRole.FK_Role=r.ID;
        addUserRole.Role=r;
        addUserRole.FK_User=this.user.ID;
        this.user.UserRoles.push(addUserRole);
      }
      else{
        ur.IsChecked=true;
      }
    });
   this.loaded=true;
  }

  roleChange(ev){
    
  }
}
