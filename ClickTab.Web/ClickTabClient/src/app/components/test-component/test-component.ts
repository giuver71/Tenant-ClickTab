import { PickerModeEnum, TimeTypeEnum } from '@eqproject/eqp-datetimerangepicker';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { UserDTO } from '../../models/generics/user.model';
import { UserService } from '../../services/user.service';
import { DialogService } from '../../services/dialog.service';

export enum GenderEnum {
  MASCHIO = 1,
  FEMMINA = 2
}

@Component({
  selector: 'app-test',
  templateUrl: './test-component.html',
  styleUrls: ['./test-component.scss']
})
export class TestComponent implements OnInit {

  genderEnum = GenderEnum;
  columns: Array<ConfigColumn>;

  formGroupExample: FormGroup;

  currentCompany: any;
  currentCompany2: any;

  companyTypes: any;

  data: Array<UserDTO> = new Array<UserDTO>();

  pickerModeEnum = PickerModeEnum;
  timeTypeEnum = TimeTypeEnum;
  placeholder: string = "Test datepicker";
  selectedTime: Date | string | null = "20:00";

  constructor(private formBuilder: FormBuilder, private userService: UserService) {
    this.formGroupExample = this.formBuilder.group({
      select: [this.currentCompany, Validators.required],
      select2: [this.currentCompany2]
    })
    this.configColumn();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsers().then(res => {
      this.data = res;
    }).catch(err => {
      DialogService.Error("Qualcosa è andato storto nella lettura degli utenti: " + err);
    })
  }

  configColumn() {
    this.columns = [
      { key: "Name", display: "Nome" },
      { key: "Surname", display: "Cognome" },
      { key: "Email", display: "indirizzo email" },
      {
        key: "SubscriptionDate", display: "Data registrazione",
        type: TypeColumn.Date, styles: { flex: "10%" }
      },
      {
        key: "ChangedPassword", display: "Password cambiata",
        type: TypeColumn.Boolean, booleanValues: { true: "Si", false: "No" }, styles: { flex: "10%" }
      },
      {
        key: "action", display: "Action",
        type: TypeColumn.MenuAction, buttonMenuIcon: "list", actions: [{ name: "update", hidden: true }, { name: "delete", disabled: (element: any) => this.disableRow(element), icon: "keyboard_arrow_left", fn: (element: any, col: any, elementIndex: any) => this.deleteRow(element, col, elementIndex) }]
      }
    ]
  }

  disableRow(element: any) {

  }

  deleteRow(element: any, col: any, elementIndex: any) {

  }

  SelectionInfo(event: any) {
    console.log(event);
  }

  onDateChange(event: any) {
    console.log(event);
  }

}
