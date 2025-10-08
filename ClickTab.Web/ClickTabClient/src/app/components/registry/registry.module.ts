import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { SharedModule } from '../../../modules/shared.module';
 
import { RegistryRoutingModule } from './registry.routing';
import { ListUsersComponent } from './users/list-users/list-users.component';
import { AddUserComponent } from './users/add-user/add-user.component';

@NgModule({
  imports: [
    FormsModule,
    RegistryRoutingModule,
    BsDropdownModule,
    ButtonsModule.forRoot(),
    SharedModule
  ],
  declarations: [
    ListUsersComponent,
    AddUserComponent
  ],
  exports: [
  ],
  providers: [
  ]
})
export class RegistryModule {
  constructor() {
  
  }
}
