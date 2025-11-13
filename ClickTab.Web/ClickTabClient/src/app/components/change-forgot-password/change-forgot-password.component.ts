import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "./../../services/auth.service";
import { DialogService } from "./../../services/dialog.service";
import { UserService } from "./../../services/user.service";
import { UserDTO } from "../../models/generics/user.model";

@Component({
  selector: "app-change-forgot-password",
  templateUrl: "./change-forgot-password.component.html",
  styleUrls: ["./change-forgot-password.component.scss"]
})
export class ChangeForgotPasswordComponent {
  changeForm: FormGroup;
  submitted = false;
  currentUser: UserDTO = new UserDTO();
  validToken: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService,
    private authService: AuthService,
    private activateRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (this.activateRoute.snapshot.params["token"] != null) {
      let data = {
        Token: this.activateRoute.snapshot.params["token"]
      };

      this.userService.checkToken(data).subscribe({
        next: (resp: any) => {
          this.validToken = true;
        },
        error: (err: any) => {
          this.validToken = false;
          DialogService.Error(err.message);
          this.authService.logout();
        }
      });

      this.changeForm = this.formBuilder.group(
        {
          email: ["", Validators.required],
          newPassword: ["", Validators.required],
          confirmPassword: ["", Validators.required]
        },
        {
          validator: this.MustMatch("newPassword", "confirmPassword")
        }
      );
    }
  }

  // Custom validator per match di 2 campi
  MustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      //Se ci sono già errori non c'è bisogno di controllare altro
      if (matchingControl.errors && !matchingControl.errors["mustMatch"]) {
        return;
      }
      //Se la validazione fallisce imposto l'errore a true
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  /**
   * Per accedere ai campi del form facilmente
   */
  get f() {
    return this.changeForm.controls;
  }

  /**
   * Al click del pulsante "Invia Password" invio l'email inserita dall'utente
   * per recuperare la password
   */
  onSubmit() {
    debugger;
    this.submitted = true;

    // Se il form non è valido mi fermo
    if (this.changeForm.invalid) {
      return;
    }

    let data = {
      Email: this.changeForm.controls["email"].value,
      Password: this.changeForm.controls["newPassword"].value,
      Token: this.activateRoute.snapshot.params["token"]
    };

    this.userService
      .resetPassword(data)
      .then((res) => {
        DialogService.Success("Password modificata");
        this.authService.logout();
      })
      .catch((err) => {
        DialogService.Error(err.message);
      });
  }
}
