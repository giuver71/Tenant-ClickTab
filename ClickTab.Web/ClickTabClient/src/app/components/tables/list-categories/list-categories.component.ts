import { Component, OnInit } from '@angular/core';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { UserDTO } from '../../../models/generics/user.model';
import { CategoryDTO } from '../../../models/tables/category.model';
import { HeaderButton } from '../../../elements/page-header/page-header.component';
import { CategoryService } from '../../../services/tables/category.services';
import { DialogService } from 'src/app/services/dialog.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-list-categories',
  templateUrl: './list-categories.component.html',
  styleUrl: './list-categories.component.scss'
})
export class ListCategoriesComponent implements OnInit{
    currentUser!:UserDTO;
    columns:Array<ConfigColumn>=new Array<ConfigColumn>();
    buttons:Array<HeaderButton>=new Array<HeaderButton>();
    categories:Array<CategoryDTO>=new Array<CategoryDTO>();

  constructor(
    private categoryService:CategoryService,
    private router:Router

  ) {
    
    
  }

  ngOnInit(): void {
    this.configureColumns();
    this.configureHeaderButtons();
    this.reloadCategories();

  }

  
     configureColumns(){
        this.columns=[
           {
            key: "action", display: "",
            type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
            actions: [
              { name: 'Modifica', icon: "edit", fn: (element:CategoryDTO) => this.editRow(element) },
             
            ]
          },
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

    editRow(el:CategoryDTO){
       this.router.navigate(['tables/add-categories/', el.ID]);
    }

    addCategory(){
       this.router.navigate(["tables/add-categories"]);
    }
    reloadCategories(){
      this.categoryService.getAll().then((res)=>{
          this.categories=res;
        }).catch((err)=>{
          console.error("list-categories.reloadCategories",err);
          DialogService.Error(err.message);
        })
    }
    
}
