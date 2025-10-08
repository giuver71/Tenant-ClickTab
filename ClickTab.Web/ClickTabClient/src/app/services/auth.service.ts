import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserDTO, UserStatusEnum } from '../models/generics/user.model';
import { environment } from './../../environments/environment';
import { RoleDTO } from '../models/generics/role.model';
import { LS_CURRENT_ROLE_ID } from '../helpers/global-consts';

const helper = new JwtHelperService();
const CURRENT_TOKEN_NAME: string = "eqpToken";

@Injectable({
  providedIn: 'root'
})

/**
 * Servizio di autenticazione
 */
export class AuthService {

  public loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.tokenAvailable());

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  constructor(
    private http: HttpClient,
    private router: Router) {
  }

  /**
   * Effettua la login e, se login completata con successo, restituisce il token
   * @param email Email dell'utente da loggare
   * @param password Password dell'utente da loggare
   */
  login(email: string, password: string): Promise<any> {

    let loginData: any = {
      Email: email,
      Password: password,
    };

    return this.http.post<any>(environment.apiUrl + '/api/auth/login', loginData).toPromise();
  }

  /**
   * Al logout rimuovo l'utente dal local storage e lo indirizzo alla pagina di Login
   */
  logout() {
    this.loggedIn.next(false);
    localStorage.removeItem(CURRENT_TOKEN_NAME);
    this.router.navigate(['/login']);
  }

  /**
   * Effettua la registrazione e, se registrazione completata con successo, restituisce il token
   * @param name Nome dell'utente da registrare
   * @param surname Cognome dell'utente da registrare
   * @param email Email dell'utente da registrare
   * @param password Password dell'utente da registrare
   */
  register(name: string, surname: string, email: string, password: string): Promise<any> {
    let user: UserDTO = new UserDTO();
    user.Name = name;
    user.Surname = surname;
    user.Email = email;
    user.Password = password;

    return this.http.post<any>(environment.apiUrl + '/api/auth/register', user).toPromise();
  }

  /**
   * Recupera lo user dal token memorizzato nel local storage
   */
  getCurrentUser(): UserDTO {
    let decodedToken = helper.decodeToken(this.getCurrentToken())
    console.log("decodetoken",decodedToken);
    let user:UserDTO=decodedToken.User as UserDTO;
   
    return decodedToken != undefined ? decodedToken.User : null;
  }

  /**
   * Recupera la lingua corrente dal token memorizzato nel local storage
   */
  getCurrentLanguage(): any {
    let decodedToken = helper.decodeToken(localStorage.getItem(CURRENT_TOKEN_NAME))
    return decodedToken.Language;
  }

  /**
   * Memorizza il token nel local storage
   * @param token Token da memorizzare nel local storage
   */
  setCurrentToken(token) {
    localStorage.setItem(CURRENT_TOKEN_NAME, token);
  }

  /**
   * Recupera il token dal local storage
   */
  getCurrentToken() {
    return localStorage.getItem(CURRENT_TOKEN_NAME);
  }

  /**
   * Restituisce TRUE se il token è disponibile
   */
  tokenAvailable(): boolean {
    let token = localStorage.getItem(CURRENT_TOKEN_NAME);
    return token != undefined && token != null;
  }

  getCurrentRole(): RoleDTO {

    const hashedRole = localStorage.getItem(LS_CURRENT_ROLE_ID);
    if(hashedRole != null && hashedRole != "null"){
      const currentRoleUnhashed = helper.decodeToken(hashedRole);
      // console.log("getCurrentRole", currentRoleUnhashed.Role)
      return currentRoleUnhashed.Role;
      // return JSON.parse(currentRoleID).Role;
    }else {
        console.log("getCurrentRole null")
        return null;
    }
  }

   setCurrentRole(roleDTO: string) {
    localStorage.setItem(LS_CURRENT_ROLE_ID, roleDTO);
   
    // aggiorna userAttachpointRole
    // this.ruleService.updateAttachpointUserRole(activeUserRoleRules);
    // regole sulle compunicazioni
    // this.ruleService.updateCommunicationUserRole(activeUserRoleRules);
  }

   decodeToken(item){
    return helper.decodeToken(item);
  }
}
