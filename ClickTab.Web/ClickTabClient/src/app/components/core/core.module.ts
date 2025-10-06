import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { SharedModule } from '../../../modules/shared.module';
import { CoreRoutingModule } from './core.routing';

@NgModule({
  imports: [
    FormsModule,
    CoreRoutingModule,
    BsDropdownModule,
    ButtonsModule.forRoot(),
    SharedModule,
  ],
  declarations: [
  ]
})
export class CoreModule { }
