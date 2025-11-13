import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { SharedModule } from '../../../modules/shared.module';
import { TablesRoutingModule } from './tables.routing';
import { ListCategoriesComponent } from './list-categories/list-categories.component';
import { AddCategoriesComponent } from './add-categories/add-categories.component';
import { ListSubcategoriesComponent } from './list-subcategories/list-subcategories.component';
import { AddSubcategoriesComponent } from './add-subcategories/add-subcategories.component';
 
 
@NgModule({
  imports: [
    FormsModule,
    TablesRoutingModule,
    BsDropdownModule,
    ButtonsModule.forRoot(),
    SharedModule
  ],
  declarations: [
    ListCategoriesComponent,
    AddCategoriesComponent,
    ListSubcategoriesComponent,
    AddSubcategoriesComponent
  ],
  exports: [
  ],
  providers: [
  ]
})
export class TablesModule {
  constructor() {
  
  }
}
