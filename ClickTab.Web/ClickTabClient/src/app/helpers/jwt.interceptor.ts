import { SpinnerService } from './../services/spinner.service';
import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

/**
 * Indicare di seguito le rotte degli endpoint per i quali non si vuole far partire lo spinner
 */
const RequestWithoutSpinnerList: Array<string> = [
    // "/api/Notification/MarkAsRead"
]

@Injectable()

/**
 * Intercetta le richieste http dall'applicazione per aggiungere un token di autenticazione JWT all'header
 * se l'utente ha effettuato l'accesso
 */
export class JwtInterceptor implements HttpInterceptor {

    // private requestsCount: number = 0;

    constructor(
        private authenticationService: AuthService,
        private spinnerService: SpinnerService) { }

    /**
     * Se presente il token JWT concedo l'autorizzazione
     * @param request 
     * @param next 
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let currentToken = this.authenticationService.getCurrentToken();
        if (currentToken) {
            request = request.clone({
                setHeaders: {
                    Authorization: `${currentToken}`
                }
            });
        }

        //Aggiunge la richiesta all'elenco delle richieste disponibili e mostra lo spinner (se l'endpoint non è inserito nelle rotte per cui nascondere lo spinner)
        if (!RequestWithoutSpinnerList.find(r => request.url.includes(r)))
            this.spinnerService.addRequestCounter();

        //Invoca l'observable e resta in ascolto della conclusione della chiamata HTTP
        //per stoppare se necessario lo spinner
        return next.handle(request).pipe(finalize(() => this.removeRequest()));
    }

    /**
     * Rimuove la richiesta e mostra lo spinner
     */
    removeRequest() {
        this.spinnerService.removeRequestCounter();
    }
}
