import { AuthService } from '../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  submitted = false;
  repeatPassword: string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthService) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
      repeatPassword: ['', Validators.required]
    });
  }

  /**
   * Per accedere ai campi del form facilmente
   */
  get f(): any {
    return this.registerForm.controls;
  }

  /**
   * Al click del pulsante "Crea account" invio le credenziali
   * per la registrazione dell'utente
   */
  onSubmit() {
    this.submitted = true;

    // Se il form non è valido mi fermo
    if (this.registerForm.invalid) {
      return;
    }

    this.authenticationService.register(this.f.name.value, this.f.surname.value, this.f.email.value, this.f.password.value)
      .then((resUser) => {

        // Registrazione effettuata con successo
        this.router.navigate(['/']);
      })
      .catch((err) => {
        Swal.fire({
          title: err,
          icon: 'error',
        })
      });
  }
}
