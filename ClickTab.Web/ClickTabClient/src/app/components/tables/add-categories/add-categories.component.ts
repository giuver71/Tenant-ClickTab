import { Component, OnInit } from '@angular/core';
import { CategoryDTO } from '../../../models/tables/category.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfigColumn } from '@eqproject/eqp-common';
import { HeaderButton } from '../../../elements/page-header/page-header.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../../services/tables/category.services';
import { DialogService } from '../../../services/dialog.service';
import { MessageBarService } from '../../../services/messagebar.service';

@Component({
  selector: 'app-add-categories',
  templateUrl: './add-categories.component.html',
  styleUrl: './add-categories.component.scss'
})
export class AddCategoriesComponent implements OnInit{
    category:CategoryDTO=new CategoryDTO();
    categoryForm!: FormGroup;
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
    private categoryService:CategoryService,
    private router:Router,

  ) {
  }
   ngOnInit() {
      if( this.activateRoute.snapshot.params["id"]!=null){
              this.getCategory();
      }  
      else{
        this.category=new CategoryDTO();
        this.configureHeaderButtons();
        this.createForm();
      }
     
    }

    createForm(){
         this.categoryForm = this.formBuilder.group({
              description: [this.category?.Description, Validators.required],
              code: [this.category?.Code, Validators.required],
              fee:[this.category?.Fee],
              department:[this.category?.Department],
              isFiscal:[this.category.IsFiscal],
              negative:[this.category.Negative],
              feeOnPurchasePrice:[this.category.FeeOnPurchasePrice]
            });
        
            this.categoryForm.statusChanges.subscribe(() => {
              if (!this.categoryForm.invalid) {
                this.buttons[0].isDisabled = false;
              }
            });
      }
    

    configureHeaderButtons() {
      this.buttons = [
      {
        label: 'Salva Gruppo',
        icon: 'fa fa-save',
        color: 'btn-secondary',
        isDisabled: true,
        action: () => this.save()
      },

    ];
  }

  getCategory(){
    this.categoryService.get(this.activateRoute.snapshot.params["id"]).then((res)=>{
      this.category=res;
       this.configureHeaderButtons();
      this.createForm();
    }).catch((err)=>{
      console.error("AddCategory.getRules()", err);
      DialogService.Error(err.message);
    })
  }

  save(){
    this.categoryService.save(this.category).then((res) => {
        this.messageBar.show({
            message: 'Gruppo salvato correttamente!',
            type: 'success',
            duration: 4000,
            actionLabel: 'Chiudi',
            onClose: () => this.router.navigate(['/tables/list-categories'])
      });
    }).catch((err) => {
      console.error("addCategory.Save", err);
      DialogService.Error(err.message);
    }) 
  }
  
 


} 
