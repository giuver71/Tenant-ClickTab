import { Component } from '@angular/core';
import { VatDTO } from '../../../../models/tables/vat.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigColumn } from '@eqproject/eqp-common';
import { HeaderButton } from '../../../../elements/page-header/page-header.component';
import { DialogService } from '../../../../services/dialog.service';
import { MessageBarService } from '../../../../services/messagebar.service';
import { VatService } from '../../../../services/tables/vat.services';
import { RoleRuleDTO } from '../../../../models/generics/rolerule.model';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-add-vats',
  templateUrl: './add-vats.component.html',
  styleUrl: './add-vats.component.scss'
})
export class AddVatsComponent {
      vat:VatDTO=new VatDTO();
      vatForm!: FormGroup;
      buttons: Array<HeaderButton> = new Array<HeaderButton>();
      columns:Array<ConfigColumn>=new Array<ConfigColumn>;
      loaded:boolean=false;
      editPercentage:boolean=false;
    /**
     *
     */
    constructor(
      private formBuilder: FormBuilder,
      private snackBar: MatSnackBar,
      private activateRoute: ActivatedRoute,
      private messageBar: MessageBarService,
      private vatService:VatService,
      private router:Router,
      private auth:AuthService
  
    ) {
    }
     ngOnInit() {
      var currentRole=this.auth.getCurrentRole();
      var currentRules=this.auth.getSpecificRule("Modifica Aliquota IVA",currentRole.RoleRules)
      this.editPercentage=currentRules!=null;
        if( this.activateRoute.snapshot.params["id"]!=null){
            this.getVat();
        }  
        else{
          this.vat=new VatDTO();
          this.configureHeaderButtons();
          this.createForm();
        }
       
      }
  
      createForm(){
           this.vatForm = this.formBuilder.group({
                description: [this.vat?.Description, Validators.required],
                code: [this.vat?.Code, Validators.required],
                percentage:[this.vat?.Percentage],
              });
          
              this.vatForm.statusChanges.subscribe(() => {
                if (!this.vatForm.invalid) {
                  this.buttons[0].isDisabled = false;
                }
              });
        }
      
  
      configureHeaderButtons() {
        this.buttons = [
        {
          label: 'Salva Tabella Iva',
          icon: 'fa fa-save',
          color: 'btn-secondary',
          isDisabled: true,
          action: () => this.save()
        },
  
      ];
    }
  
    getVat(){
      this.vatService.get(this.activateRoute.snapshot.params["id"]).then((res)=>{
        this.vat=res;
         this.configureHeaderButtons();
        this.createForm();
      }).catch((err)=>{
        console.error("AddVat.getVat()", err);
        DialogService.Error(err.message);
      })
    }
  
    save(){
      this.vatService.save(this.vat).then((res) => {
          this.messageBar.show({
              message: 'Tabella Iva correttamente!',
              type: 'success',
              duration: 4000,
              actionLabel: 'Chiudi',
              onClose: () => this.router.navigate(['/tables/list-vats'])
        });
      }).catch((err) => {
        console.error("addVat.Save", err);
        DialogService.Error(err.message);
      }) 
    }
}
