import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { SharedModule } from '../../../modules/shared.module';
 
import { RegistryRoutingModule } from './registry.routing';
import { ListUsersComponent } from './users/list-users/list-users.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { ListRolesComponent } from './roles/list-roles/list-roles.component';
import { AddRolesComponent } from './roles/add-roles/add-roles.component';

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
    AddUserComponent,
    ListRolesComponent,
    AddRolesComponent
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
