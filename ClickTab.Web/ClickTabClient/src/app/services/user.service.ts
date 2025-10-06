import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { environment } from "../../environments/environment";
import { UserDTO } from "../models/generics/user.model";

@Injectable({ providedIn: "root" })

/**
 * Servizio per utenti
 */
export class UserService {
  constructor(private http: HttpClient) {}

  /**
   * Metodo per ottenere tutti gli utenti dall'API
   */

  getAllUsers(): Promise<Array<UserDTO>> {
    return this.http.get<Array<UserDTO>>(environment.apiFullUrl + "/User/GetAllUsers").toPromise();
  }

  getUserByID(id: number): Promise<UserDTO> {
    return this.http.get<UserDTO>(environment.apiFullUrl + "/User/" + id).toPromise();
  }

  saveUser(user: UserDTO): Promise<any> {
    return this.http.post<any>(environment.apiFullUrl + "/User", user).toPromise();
  }

  deleteUser(id: number): Promise<any> {
    return this.http.delete<any>(environment.apiFullUrl + "/User/" + id).toPromise();
  }

  firstLoginResetPassword(data: any): Promise<any> {
    return this.http.post<any>(environment.apiFullUrl + "/User/FirstLoginResetPassword", data).toPromise();
  }

  enableResetPassword(data: any): Promise<any> {
    return this.http.post<any>(environment.apiFullUrl + "/User/EnableResetPassword", data).toPromise();
  }

  checkToken(data: any): Observable<any> {
    return this.http.post<any>(environment.apiFullUrl + "/User/CheckToken", data).pipe(
      catchError((error: any) => {
        return throwError(() => error);
      })
    );
  }

  resetPassword(data: any): Promise<any> {
    return this.http.post<any>(environment.apiFullUrl + "/User/ResetPassword", data).toPromise();
  }
}
