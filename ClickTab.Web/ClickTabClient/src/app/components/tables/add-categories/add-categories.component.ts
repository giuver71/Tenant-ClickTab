import { Component, OnInit } from '@angular/core';
import { CategoryDTO } from '../../../models/tables/category.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfigColumn } from '@eqproject/eqp-common';
import { HeaderButton } from '../../../elements/page-header/page-header.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { MessageBarService } from 'src/app/services/messagebar.service';
import { CategoryService } from '../../../services/tables/category.services';
import { DialogService } from 'src/app/services/dialog.service';

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
              isFiscal:[this.category.IsFiscal]
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

  }
  
  onToggleChange(cat: any) {
    // Questo triggera il binding per aggiornare colore e messaggio
    cat.IsFiscal = !cat.IsFiscal;
  }
  


} 
