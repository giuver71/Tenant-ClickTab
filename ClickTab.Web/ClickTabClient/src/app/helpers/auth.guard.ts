import { Injectable } from "@angular/core";
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(public router: Router, private authService: AuthService) {
  }

  /**
   * Verifica soltanto se è presente il token di autenticazione
   */
  canActivate(): boolean {
    // if (this.authService.tokenAvailable() != true || this.authService.getCurrentUser() == null) {
    //   //Utente non loggato e quindi redirect alla pagina di Login
    //   this.router.navigate(['/login']);
    //   return false;
    // }

    //Utente loggato
    return true;
  }

}
