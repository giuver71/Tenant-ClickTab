import { Component, OnInit } from '@angular/core';
import { UserDTO, UserStatusEnum } from '../../../../models/generics/user.model';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { UserService } from '../../../../services/user.service';
import { DialogService } from '../../../../services/dialog.service';
import { AuthService } from '../../../../services/auth.service';
import { HeaderButton } from '../../../../elements/page-header/page-header.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.scss'
})
export class ListUsersComponent implements OnInit{
  currentUser!:UserDTO;
  users:Array<UserDTO>=new Array<UserDTO>();
  columns:Array<ConfigColumn>=new Array<ConfigColumn>();
  buttons:Array<HeaderButton>=new Array<HeaderButton>();
  constructor(
    private userService:UserService,
    private authService:AuthService,
    private router:Router

  ) {
    
  }

  ngOnInit(): void {
    this.currentUser=this.authService.getCurrentUser();
    this.configureHeaderButtons();
    this.configureColumns();
    this.reloadUsers();
  }

  configureHeaderButtons(){
    this.buttons = [
      {
        label: 'Aggiungi Nuovo Utente',
        icon: 'fa fa-plus',
        color: 'btn-secondary',
        action: () => this.addUser()
      },
     
    ];
  }


  configureColumns(){
    this.columns=[
       {
        key: "action", display: "",
        type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
        actions: [
          { name: 'Modifica', icon: "edit", fn: (element:UserDTO) => this.editRow(element) },
           { name: 'Abilita Utente', icon: "book", fn: (element:UserDTO) => this.enableUser(element),
              hidden: (element: UserDTO) => element.Status==UserStatusEnum.Abilitato },
          { name: 'Disabilita Utente', icon: "close", fn: (element:UserDTO) => this.disableUser(element),
              hidden: (element: UserDTO) => element.Status==UserStatusEnum.Disabilitato }
        ]
      },
      { key: "Name", display: 'Nome',styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT } },
      { key: "Surname", display: 'Cognome', styles: { flex: "0 0 20%",cellAlignment:CellAlignmentEnum.LEFT }  },
      { key: "Email", display: 'E-mail', styles: { flex: "0 0 10%",cellAlignment:CellAlignmentEnum.LEFT }  },
      { key: "Status", display: 'Stato',
        type: TypeColumn.Enum, enumModel: UserStatusEnum ,
        styles:{flex: "0 0 10%"},
        containerStyle: (element:UserDTO) => {return this.getUserStatusStyle(element)}
      },

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
    this.router.navigate(['registry/add-users/', el.ID]);
  }



  getUserStatusStyle(element : UserDTO){
    switch(element.Status) {
      case UserStatusEnum.Abilitato:
        return { padding:"5px", color: "white", background: "green"  };
      case UserStatusEnum.Disabilitato:
        return { padding:"5px", color: "white", background: "red"  };
     
    }
  }

  addUser(){
   this.router.navigate(["registry/add-users"]);
  }

  refresh(){

  }

  enableUser(el:UserDTO){
    this.userService.changeStatus(UserStatusEnum.Abilitato,el.ID).then((res)=>{
      DialogService.Success("Utente Abilitato");
      this.reloadUsers();
    }).catch((err)=>{
      console.error("list-users.enableUser",err);
      DialogService.Error(err.message);      
    })
  }

  disableUser(el:UserDTO){
    this.userService.changeStatus(UserStatusEnum.Disabilitato,el.ID).then((res)=>{
      DialogService.Success("Utente Disabilitato");
      this.reloadUsers();
    }).catch((err)=>{
      console.error("list-users.disableUser",err);
      DialogService.Error(err.message);      
    })
  }

}
