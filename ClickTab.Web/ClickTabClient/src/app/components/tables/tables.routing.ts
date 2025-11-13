import { NgModule } from '@angular/core';
import { Routes, RouterModule, RouterStateSnapshot } from '@angular/router';
import { PermissionGuard } from "../../helpers/permission.guard";
import { ListCategoriesComponent } from "./list-categories/list-categories.component"
import { AddCategoriesComponent } from "./add-categories/add-categories.component"
import { ListSubcategoriesComponent } from './list-subcategories/list-subcategories.component';
import { AddSubcategoriesComponent } from './add-subcategories/add-subcategories.component';
const routes: Routes = [
  {
    path: 'list-categories',
    component: ListCategoriesComponent,
    canActivate: [PermissionGuard],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  }, 
  {
    path: 'add-categories',
    component: AddCategoriesComponent,
    canActivate: [PermissionGuard],
    canDeactivate: [],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  },
  {
    path: 'add-categories/:id',
    component: AddCategoriesComponent,
    canActivate: [PermissionGuard],
    canDeactivate: [],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  },
 {
    path: 'list-subcategories',
    component: ListSubcategoriesComponent,
    canActivate: [PermissionGuard],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  }, 
  {
    path: 'add-subcategories',
    component: AddSubcategoriesComponent,
    canActivate: [PermissionGuard],
    canDeactivate: [],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  },
  {
    path: 'add-subcategories/:id',
    component: AddSubcategoriesComponent,
    canActivate: [PermissionGuard],
    canDeactivate: [],
    data: {
      breadcrumbs: 'BREADCRUMB.ROLES',
      state: RouterStateSnapshot
    },
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TablesRoutingModule { }
