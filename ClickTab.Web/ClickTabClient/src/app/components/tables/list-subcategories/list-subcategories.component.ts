import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConfigColumn, TypeColumn, CellAlignmentEnum } from '@eqproject/eqp-common';
import { HeaderButton } from '../../../elements/page-header/page-header.component';
import { DialogService } from '../../../services/dialog.service';
import { CategoryService } from '../../../services/tables/category.services';
import { SubCategoryDTO } from '../../../models/tables/subcategory.model';
import { SubCategoryService } from '../../../services/tables/subcategory.services';

@Component({
  selector: 'app-list-subcategories',
  templateUrl: './list-subcategories.component.html',
  styleUrl: './list-subcategories.component.scss'
})
export class ListSubcategoriesComponent {
      columns:Array<ConfigColumn>=new Array<ConfigColumn>();
      buttons:Array<HeaderButton>=new Array<HeaderButton>();
      subCategories:Array<SubCategoryDTO>=new Array<SubCategoryDTO>();
  
    constructor(
      private subCategoryService:SubCategoryService,
      private router:Router
  
    ) {
      
      
    }
  
    ngOnInit(): void {
      this.configureColumns();
      this.configureHeaderButtons();
      this.reloadSubCategories();
  
    }
  
    
       configureColumns(){
          this.columns=[
             {
              key: "action", display: "",
              type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
              actions: [
                { name: 'Modifica', icon: "edit", fn: (element:SubCategoryDTO) => this.editRow(element) },
               
              ]
            },
            { key: "Code", display: 'Codice',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
            { key: "Description", display: 'Descrizione',styles: { flex: "0 0 50%",cellAlignment:CellAlignmentEnum.LEFT } },
          ];
        }
    
       configureHeaderButtons(){
        this.buttons = [
          {
            label: 'Aggiungi Nuovo Gruppo',
            icon: 'fa fa-plus',
            color: 'btn-secondary',
            action: () => this.addCategory()
          },
         
        ];
      }
  
      editRow(el:SubCategoryDTO){
         this.router.navigate(['tables/add-subcategories/', el.ID]);
      }
  
      addCategory(){
         this.router.navigate(["tables/add-subcategories"]);
      }
      reloadSubCategories(){
        this.subCategoryService.getAll().then((res)=>{
            this.subCategories=res;
          }).catch((err)=>{
            console.error("list-subcategories.reloadSubCategories",err);
            DialogService.Error(err.message);
          })
      }
}
