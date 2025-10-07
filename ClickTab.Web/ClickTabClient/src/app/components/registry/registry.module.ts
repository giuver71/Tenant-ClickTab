import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { SharedModule } from '../../../modules/shared.module';
 
import { RegistryRoutingModule } from './registry.routing';
import { ListUsersComponent } from './list-users/list-users.component';

@NgModule({
  imports: [
    FormsModule,
    RegistryRoutingModule,
    BsDropdownModule,
    ButtonsModule.forRoot(),
    SharedModule
  ],
  declarations: [
    ListUsersComponent
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
