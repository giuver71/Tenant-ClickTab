import { Component } from '@angular/core';
import { DialogService } from '../../../services/dialog.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SubCategoryDTO } from '../../../models/tables/subcategory.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigColumn } from '@eqproject/eqp-common';
import { HeaderButton } from '../../../elements/page-header/page-header.component';
import { CategoryDTO } from '../../../models/tables/category.model';
import { MessageBarService } from '../../../services/messagebar.service';
import { CategoryService } from '../../../services/tables/category.services';
import { SubCategoryService } from '../../../services/tables/subcategory.services';

@Component({
  selector: 'app-add-subcategories',
  templateUrl: './add-subcategories.component.html',
  styleUrl: './add-subcategories.component.scss'
})
export class AddSubcategoriesComponent {
      subCategory:SubCategoryDTO=new SubCategoryDTO();
      subCategoryForm!: FormGroup;
      buttons: Array<HeaderButton> = new Array<HeaderButton>();
      columns:Array<ConfigColumn>=new Array<ConfigColumn>;
      loaded:boolean=false;
  
    /**
     *
     */
    constructor(
      private formBuilder: FormBuilder,
      private snackBar: MatSnackBar,
      private activateRoute: ActivatedRoute,
      private messageBar: MessageBarService,
      private subCategoryService:SubCategoryService,
      private router:Router,
  
    ) {
    }
     ngOnInit() {
        if( this.activateRoute.snapshot.params["id"]!=null){
                this.getSubCategory();
        }  
        else{
          this.subCategory=new SubCategoryDTO();
          this.configureHeaderButtons();
          this.createForm();
        }
       
      }
  
      createForm(){
           this.subCategoryForm = this.formBuilder.group({
                description: [this.subCategory?.Description, Validators.required],
                code: [this.subCategory?.Code, Validators.required],
              });
          
              this.subCategoryForm.statusChanges.subscribe(() => {
                if (!this.subCategoryForm.invalid) {
                  this.buttons[0].isDisabled = false;
                }
              });
        }
      
  
      configureHeaderButtons() {
        this.buttons = [
        {
          label: 'Salva Sotto Gruppo',
          icon: 'fa fa-save',
          color: 'btn-secondary',
          isDisabled: true,
          action: () => this.save()
        },
  
      ];
    }
  
    getSubCategory(){
      this.subCategoryService.get(this.activateRoute.snapshot.params["id"]).then((res)=>{
        this.subCategory=res;
         this.configureHeaderButtons();
        this.createForm();
      }).catch((err)=>{
        console.error("AddSubCategories.getSubCategory()", err);
        DialogService.Error(err.message);
      })
    }
  
    save(){
      this.subCategoryService.save(this.subCategory).then((res) => {
          this.messageBar.show({
              message: 'Sotto Gruppo salvato correttamente!',
              type: 'success',
              duration: 4000,
              actionLabel: 'Chiudi',
              onClose: () => this.router.navigate(['/tables/list-subcategories'])
        });
      }).catch((err) => {
        console.error("add-subcategories.Save", err);
        DialogService.Error(err.message);
      }) 
    }
    
}
