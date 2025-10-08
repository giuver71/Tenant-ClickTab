import { Component, OnInit } from '@angular/core';
import { UserDTO } from '../../../models/generics/user.model';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { UserService } from '../../../services/user.service';
import { DialogService } from '../../../services/dialog.service';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.scss'
})
export class ListUsersComponent implements OnInit{

  users:Array<UserDTO>=new Array<UserDTO>();
  columns:Array<ConfigColumn>=new Array<ConfigColumn>();

  constructor(
    private userService:UserService

  ) {
    
  }

  ngOnInit(): void {
  
    this.configureColumns();
    this.reloadUsers();
  }
  configureColumns(){
    this.columns=[
       {
        key: "action", display: "",
        type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
        actions: [
          { name: 'Modifica', icon: "edit", fn: (element:UserDTO) => this.editRow(element) },
        ]
      },
      { key: "Name", display: 'Nome',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
      { key: "Surname", display: 'Cognome', styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT }  },
      { key: "Email", display: 'E-mail', styles: { flex: "0 0 10%",cellAlignment:CellAlignmentEnum.LEFT }  },

    ];
  }

  reloadUsers(){
    this.userService.getAllUsers().then((res)=>{
      this.users=res;
    }).catch((err)=>{
      console.error("list-users.reloadUsers",err);
      DialogService.Error(err.message);
    })
  }

  editRow(el:UserDTO){

  }

}
