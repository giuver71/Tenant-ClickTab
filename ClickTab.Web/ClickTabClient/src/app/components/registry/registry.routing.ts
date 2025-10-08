import { NgModule } from '@angular/core';
import { Routes, RouterModule, RouterStateSnapshot } from '@angular/router';
import { ListUsersComponent } from './users/list-users/list-users.component';
import { AuthGuard } from '../../helpers/auth.guard';
import { PermissionGuard } from '../../helpers/permission.guard';
import { AddUserComponent } from './users/add-user/add-user.component';
const routes: Routes = [
  // {
  //   path: 'list-roles',
  //   component: ListRolesComponent,
  //   canActivate: [PermissionGuard],
  //   data: {
  //     breadcrumbs: 'BREADCRUMB.ROLES',
  //     state: RouterStateSnapshot
  //   },
  // }, 
  // {
  //   path: 'add-roles',
  //   component: AddRolesComponent,
  //   canActivate: [PermissionGuard],
  //   canDeactivate: [FormGuard],
  //   data: {
  //     breadcrumbs: 'BREADCRUMB.ROLES',
  //     state: RouterStateSnapshot
  //   },
  // },
  // {
  //   path: 'add-roles/:id',
  //   component: AddRolesComponent,
  //   canActivate: [PermissionGuard],
  //   canDeactivate: [FormGuard],
  //   data: {
  //     breadcrumbs: 'BREADCRUMB.ROLES',
  //     state: RouterStateSnapshot
  //   },
  // },
  {
    path: 'list-users',
    component: ListUsersComponent,
    canActivate: [PermissionGuard],
    data: {
      breadcrumbs: 'Lista Utenti',
      state: RouterStateSnapshot
    },
  },
  {
    path: 'add-users',
    component: AddUserComponent,
    canActivate: [PermissionGuard],
    data: {
      breadcrumbs: 'Aggiungi Utente',
      state: RouterStateSnapshot,
      isAdminRoute: true
    },
  },
  {
    path: 'add-users/:id',
    component: AddUserComponent,
    canActivate: [PermissionGuard],
    data: {
      breadcrumbs: 'Modifica Utente',
      state: RouterStateSnapshot,
    },
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistryRoutingModule { }
