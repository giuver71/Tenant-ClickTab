import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SpinnerService } from '../services/spinner.service';
import { Router } from '@angular/router';
import { DialogService } from '../services/dialog.service';

@Injectable()

/**
 * Intercetta le risposte http dall'API per verificare la presenza di errori
 */
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthService, private spinnerService: SpinnerService, private router: Router) { }

  /**
   * Ascolto di ogni errore che arriva dal server
   * @param request
   * @param next
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      //Se si tratta di un errore di tipo UNAUTHORIZED allora naviga direttamente alla login
      //Se si tratta di un errore con codice 999 (Concurrency Exception) allora mostra il messaggio
      //Altrimenti rigira l'errore al componente che ha effettuato la chiamata
      if (err.status === 401) {
        // auto logout se ritorna errore 401 dall'API
        this.authenticationService.logout();
        location.reload();
      }
      else if (err.status === 999) {
        this.spinnerService.removeRequestCounter();
        DialogService.Confirm("Qualche altro utente ha modificato questo record. Vuoi ricaricare i dati con le ultime modifiche apportate? Cliccando su SI perderai tutti i dati che hai modificato, se clicchi su NO resterai su questa pagina ma non si potrà procedere col salvataggio", () => {
          //Ricarica la rotta corrente
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.onSameUrlNavigation = 'reload';
          this.router.navigate([this.router.url]);
        }, true, "Attenzione!");
      }
      else if (err.status === 998) {
        ///TODO: tradurre la chiave riportata all'interno di err.error.message con TranslateService e riassegnarla alla proprietà err.error.message
        //err.error.message = this.translate.instant(err.error.message);
        return throwError(() => new Error(err.error))
      }
      else
        return throwError(() => new Error(err.error.message))
    }))
  }
}
