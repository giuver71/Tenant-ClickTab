import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { DialogService } from "src/app/services/dialog.service";
import { UserService } from "src/app/services/user.service";

@Component({
  selector: "app-forgot",
  templateUrl: "./forgot.component.html",
  styleUrls: ["./forgot.component.scss"]
})
export class ForgotComponent implements OnInit {
  forgotForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder, private userService: UserService, private router: Router) {}

  ngOnInit() {
    this.forgotForm = this.formBuilder.group({
      email: ["", Validators.required]
    });
  }

  /**
   * Per accedere ai campi del form facilmente
   */
  get f() {
    return this.forgotForm.controls;
  }

  /**
   * Al click del pulsante "Invia Password" invio l'email inserita dall'utente
   * per recuperare la password
   */
  onSubmit() {
    this.submitted = true;

    // Se il form non è valido mi fermo
    if (this.forgotForm.invalid) {
      return;
    }

    let data = { ID: null, Email: this.forgotForm.value.email, Password: null, OldPassword: null };

    DialogService.Confirm(
      "Effettuare il reset della password?",
      () => {
        this.userService
          .enableResetPassword(data)
          .then((res) => {
            this.router.navigate(["/login"]);
            DialogService.Success("Email inviata, segui le istruzioni al suo interno per il reset della password.");
          })
          .catch((err) => {
            DialogService.Error(err.message);
          });
      },
      true
    );
  }
}
