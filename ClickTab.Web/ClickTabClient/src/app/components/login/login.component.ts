import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from './../../services/auth.service';
import { DialogService } from '../../services/dialog.service';

const LOCAL_STORAGE_CREDENTIAL: string = "eqpCredential";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']

})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  submitted = false;
  returnUrl: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthService
  ) {
    // Se l'utente è già connesso faccio il redirect alla Home
    if (this.authenticationService.tokenAvailable()) {
      this.router.navigate(['/'])
    }
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      tenant:['',Validators.required],
      remember: ['']
    });

    this.reloadCredentialIfExists();

    // Prendo il return url dai route parameteres o il default '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  /**
   * Se le credenziali risultano memorizzate nel local storage allora le recupera e popola i controlli per la login
   */
  reloadCredentialIfExists() {
    var credentialEncoded = localStorage.getItem(LOCAL_STORAGE_CREDENTIAL);
    if (credentialEncoded != null) {
      var credentialDecoded = JSON.parse(atob(credentialEncoded));
      this.loginForm.patchValue({ email: credentialDecoded["email"], password: credentialDecoded["password"], remember: true });
    }
  }

  /**
   * Per accedere ai campi del form facilmente
   */
  get f() {
    return this.loginForm.controls;
  }

  /**
   * Al click del pulsante "Accedi" invio le credenziali
   * per effettuare il login all'utente
   */
  onSubmit() {
    this.submitted = true;

    // Se il form non è valido mi fermo
    if (this.loginForm.invalid) {
      return;
    }

    //Se richiesto, memorizza le credenziali nel localStorage
    if (this.f.remember.value == true) {
      var credential = { email: this.f.email.value, password: this.f.password.value };
      var credentialEncoded = btoa(JSON.stringify(credential));
      localStorage.setItem(LOCAL_STORAGE_CREDENTIAL, credentialEncoded);
    }
    else
      localStorage.removeItem(LOCAL_STORAGE_CREDENTIAL);

    localStorage.setItem('tenant',this.f.tenant.value)
    
    this.authenticationService.login(this.f.email.value, this.f.password.value)
      .then((token) => {
        this.authenticationService.setCurrentToken(token);
        this.authenticationService.loggedIn.next(true);
        this.authenticationService.setCurrentRole(null);
        //Decommentare il blocco sottostante se si vuole gestire il redirect
        //al cambio password quando l'utente accede per la prima volta
        //if (this.authenticationService.getCurrentUser().ChangedPassword != true) {
        //  this.router.navigate(['/changePassword']);
        //} else {
          this.router.navigate(['/dashboard']);
        //}
      })
      .catch((err) => {
        this.authenticationService.loggedIn.next(false);
        DialogService.Error(err.message);
      });
  }
}
