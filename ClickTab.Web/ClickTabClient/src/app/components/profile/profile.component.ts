import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  profileForm: FormGroup;
  submitted = false;

  mobile = false;

  constructor(
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.profileForm = this.formBuilder.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
      repeatPassword: ['', Validators.required]
    });

    if (window.screen.width <= 480) {
      this.mobile = true;
    }

  }

  /**
   * Per accedere ai campi del form facilmente
   */
  get f() {
    return this.profileForm.controls;
  }

  /**
   * Al click del pulsante "Salva" invio le modifiche
   * del profilo utente al server
   */
  onSubmit() {
    this.submitted = true;

    // Se il form non è valido mi fermo
    if (this.profileForm.invalid) {
      return;
    }
  }

}
