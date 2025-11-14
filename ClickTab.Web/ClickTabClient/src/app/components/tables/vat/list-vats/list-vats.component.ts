import { Component } from '@angular/core';
import { VatDTO } from '../../../../models/tables/vat.model';
import { DialogService } from '../../../../services/dialog.service';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { Router } from '@angular/router';
import { HeaderButton } from '../../../../elements/page-header/page-header.component';
import { VatService } from '../../../../services/tables/vat.services';
import { RoleDTO } from '../../../../models/generics/role.model';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-list-vats',
  templateUrl: './list-vats.component.html',
  styleUrl: './list-vats.component.scss'
})
export class ListVatsComponent {
      columns:Array<ConfigColumn>=new Array<ConfigColumn>();
      buttons:Array<HeaderButton>=new Array<HeaderButton>();
      vats:Array<VatDTO>=new Array<VatDTO>();
      currentRole:RoleDTO=new RoleDTO();
    constructor(
      private vatService:VatService,
      private router:Router,
      private auth:AuthService
    ) {
      
      
    }
  
    ngOnInit(): void {
    
      console.log("Role",this.currentRole);
      this.configureColumns();
      this.configureHeaderButtons();
      this.reloadVats();
  
    }
  
    
       configureColumns(){
          this.columns=[
             {
              key: "action", display: "",
              type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
              actions: [
                { name: 'Modifica', icon: "edit", fn: (element:VatDTO) => this.editRow(element) },
               
              ]
            },
            { key: "Code", display: 'Codice',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
            { key: "Description", display: 'Descrizione',styles: { flex: "0 0 40%",cellAlignment:CellAlignmentEnum.LEFT } },
            { key: "Percentage", display: 'Aliquota',styles: { flex: "0 0 10%",cellAlignment:CellAlignmentEnum.LEFT } },
          ];
        }
    
       configureHeaderButtons(){
        this.buttons = [
          {
            label: 'Aggiungi Nuovo Tabella Iva',
            icon: 'fa fa-plus',
            color: 'btn-secondary',
            action: () => this.addVat()
          },
         
        ];
      }
  
      editRow(el:VatDTO){
         this.router.navigate(['tables/add-vats/', el.ID]);
      }
  
      addVat(){
         this.router.navigate(["tables/add-vats"]);
      }
      reloadVats(){
        this.vatService.getAll().then((res)=>{
            this.vats=res;
          }).catch((err)=>{
            console.error("list-vats.reloadVats",err);
            DialogService.Error(err.message);
          })
      }
}
