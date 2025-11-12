import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CellAlignmentEnum, ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { HeaderButton } from 'src/app/elements/page-header/page-header.component';
import { RoleDTO } from 'src/app/models/generics/role.model';
import { UserDTO } from 'src/app/models/generics/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { DialogService } from 'src/app/services/dialog.service';
import { RoleService } from 'src/app/services/generics/role.services';

@Component({
  selector: 'app-list-roles',
  templateUrl: './list-roles.component.html',
  styleUrl: './list-roles.component.scss'
})
export class ListRolesComponent implements OnInit {
  currentUser!:UserDTO;
  columns:Array<ConfigColumn>=new Array<ConfigColumn>();
  buttons:Array<HeaderButton>=new Array<HeaderButton>();
  roles:Array<RoleDTO>=new Array<RoleDTO>();

constructor(
    private roleService:RoleService,
    private authService:AuthService,
    private router:Router

) {

}
  ngOnInit(): void {
    this.configureColumns();
    this.configureHeaderButtons();
    this.reloadRoles();

  }

   configureColumns(){
      this.columns=[
         {
          key: "action", display: "",
          type: TypeColumn.MenuAction, buttonMenuIcon: "settings",
          actions: [
            { name: 'Modifica', icon: "edit", fn: (element:RoleDTO) => this.editRow(element) },
           
          ]
        },
        { key: "Description", display: 'Descrizione',styles: { flex: "0 0 50%",cellAlignment:CellAlignmentEnum.LEFT } },
      ];
    }

   configureHeaderButtons(){
    this.buttons = [
      {
        label: 'Aggiungi Nuovo Ruolo',
        icon: 'fa fa-plus',
        color: 'btn-secondary',
        action: () => this.addRole()
      },
     
    ];
  }

  reloadRoles(){
       this.roleService.getAll().then((res:Array<RoleDTO>)=>{
          this.roles=res;
        }).catch((err:any)=>{
          console.error("list-roles.reloadRoles",err);
          DialogService.Error(err.message);
        })
  }

  addRole(){
     this.router.navigate(["registry/add-roles"]);
  }

  editRow(el:RoleDTO){
     this.router.navigate(['registry/add-roles/', el.ID]);
  }

}
