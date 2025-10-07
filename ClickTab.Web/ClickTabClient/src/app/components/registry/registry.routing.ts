import { NgModule } from '@angular/core';
import { Routes, RouterModule, RouterStateSnapshot } from '@angular/router';
import { ListUsersComponent } from './list-users/list-users.component';
import { AuthGuard } from '../../helpers/auth.guard';
import { PermissionGuard } from '../../helpers/permission.guard';
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
    canActivate: [AuthGuard,PermissionGuard],
    data: {
      breadcrumbs: 'BREADCRUMB.USERS',
      state: RouterStateSnapshot
    },
  },
  // {
  //   path: 'add-users',
  //   component: AddUsersComponent,
  //   canActivate: [PermissionGuard],
  //   canDeactivate: [FormGuard],
  //   data: {
  //     breadcrumbs: 'BREADCRUMB.USERS',
  //     state: RouterStateSnapshot,
  //     isAdminRoute: true
  //   },
  // },
  // {
  //   path: 'add-users/:id',
  //   component: AddUsersComponent,
  //   canActivate: [PermissionGuard],
  //   canDeactivate: [FormGuard],
  //   data: {
  //     breadcrumbs: 'BREADCRUMB.USERS',
  //     state: RouterStateSnapshot,
  //     isAdminRoute: true
  //   },
  // },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistryRoutingModule { }
