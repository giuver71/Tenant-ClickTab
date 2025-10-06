import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { UserDTO } from "src/app/models/generics/user.model";
import { AuthService } from "./../../services/auth.service";
import { DialogService } from "./../../services/dialog.service";
import { UserService } from "./../../services/user.service";

@Component({
  selector: "app-change-password",
  templateUrl: "./change-password.component.html",
  styleUrls: ["./change-password.component.scss"]
})
export class ChangePasswordComponent implements OnInit {
  changeForm: FormGroup;
  submitted = false;
  currentUserEmail: UserDTO = this.authenticationService.getCurrentUser();

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService,
    private authenticationService: AuthService
  ) {}

  ngOnInit(): void {
    this.changeForm = this.formBuilder.group(
      {
        oldPassword: ["", Validators.required],
        newPassword: ["", Validators.required],
        confirmPassword: ["", Validators.required]
      },
      {
        validator: this.MustMatch("newPassword", "confirmPassword")
      }
    );
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
    // Se il form non è valido mi fermo
    if (this.changeForm.invalid) {
      return;
    }
    if (this.currentUserEmail.Password == this.changeForm.controls["oldPassword"].value) {
      DialogService.Error("La password vecchia e uguale a quella nuova");
      return;
    }
    let data = {
      ID: this.currentUserEmail.ID,
      Email: this.currentUserEmail.Email,
      NewPassword: this.changeForm.controls["newPassword"].value,
      OldPassword: this.changeForm.controls["oldPassword"].value
    };
    DialogService.Confirm(
      "Una volta modificata la password sarà necessario eseguire nuovamente l'accesso al sistema.",
      () => {
        this.userService
          .firstLoginResetPassword(data)
          .then((res) => {
            DialogService.Success("Password modificata");
            this.authenticationService.logout();
          })
          .catch((err) => {
            DialogService.Error(err.message);
          });
      },
      true
    );
  }
}
