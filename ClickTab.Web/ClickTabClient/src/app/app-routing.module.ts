import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ChangePasswordComponent } from "./components/change-password/change-password.component";
import { ErrorComponent } from "./components/error/error.component";
import { ForgotComponent } from "./components/forgot/forgot.component";
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";

import { ChangeForgotPasswordComponent } from "./components/change-forgot-password/change-forgot-password.component";
import { ListNotificationsComponent } from "./components/notifications/list-notifications/list-notifications.component";
import { ProfileComponent } from "./components/profile/profile.component";
import { TestComponent } from "./components/test-component/test-component";
import { DefaultLayoutComponent } from "./containers";

const routes: Routes = [
  {
    path: "404",
    component: ErrorComponent,
    data: {
      breadcrumbs: "Error 404"
    }
  },
  {
    path: "login",
    component: LoginComponent,
    data: {
      breadcrumbs: "Login Page"
    }
  },
  {
    path: "changePassword",
    component: ChangePasswordComponent,
    data: {
      breadcrumbs: "Change Password"
    }
  },
  {
    path: "register",
    component: RegisterComponent,
    data: {
      breadcrumbs: "Register Page"
    }
  },
  {
    path: "forgot",
    component: ForgotComponent,
    data: {
      breadcrumbs: "Errore 404"
    }
  },
  {
    path: "changeForgotPassword/:token",
    component: ChangeForgotPasswordComponent,
    data: {
      title: "Cambio Password"
    }
  },
  {
    path: "",
    component: DefaultLayoutComponent,
    data: {
      title: "Home"
    },
    children: [
      {
        path: "dashboard",
        data: {
          title: "Dashboard"
        },
        loadChildren: () => import("./components/dashboard/dashboard.module").then((m) => m.DashboardModule)
      },
      {
        path: "core",
        loadChildren: () => import("./components/core/core.module").then((m) => m.CoreModule)
      },
      {
        path: "profile",
        component: ProfileComponent,
        data: {
          title: "Profilo utente"
        }
      },
      {
        path: "list-notifications",
        component: ListNotificationsComponent,
        data: {
          title: "Notifiche"
        }
      },
      {
        path: "test",
        component: TestComponent,
        data: {
          title: "Test"
        }
      }
    ]
  },
  { path: "**", component: ErrorComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: "top",
      anchorScrolling: "enabled",
      initialNavigation: "enabledBlocking"
      // relativeLinkResolution: 'legacy'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
