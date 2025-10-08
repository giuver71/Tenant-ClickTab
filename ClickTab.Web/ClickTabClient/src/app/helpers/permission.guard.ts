import { Injectable } from "@angular/core";
import { Router, CanActivate, ActivatedRoute, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { TranslateService } from "@ngx-translate/core";
// import { RoleDimensionEnum } from "../models/enums/roleDimensioEnum";
import { MenuDTO } from "../models/generics/menu.model";
import { RoleDTO } from "../models/generics/role.model";
import { UserDTO } from "../models/generics/user.model";
import { AuthService } from '../services/auth.service';
import { DialogService } from "../services/dialog.service";
// import { isNumeric } from 'rxjs/internal/utils';
import { RoleRuleDTO } from "../models/generics/rolerule.model";

const CURRENT_TUTORED_USER: string = "currentTutoredUser";
const CURRENT_TUTORED_PERMISSIONS: string = "currentTutoredPermissions";

@Injectable({
  providedIn: 'root'
})
/**
 * Guardia che si occupa di controllare se l'utente ha i permessi per visitare quella pagina. In caso contrario lo riporta alla dashboard
 */
export class PermissionGuard implements CanActivate {

  constructor(public router: Router, public activatedRoute: ActivatedRoute, private translate: TranslateService, 
    private authService: AuthService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let allowed = false;
    var urlPart = state.url.split("?")[0].split("/");
    var currentUrl = "";
    var guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

    // Se la URL contiene come ultima parte un numero (Es: /registry/add-user/1234) o è una GUID, rimuove quest'ultima parte. Per sicurezza controlliamo 5 livelli di directory
      var level = 0;

      //Number.isInteger(Number(urlPart[urlPart.length - 1]))
      while (Number.isInteger(Number(urlPart[urlPart.length - 1])) || guidPattern.test(urlPart[urlPart.length - 1]) && level < 5){
      urlPart.splice(urlPart.length - 1,1);
      level++;
    }
    currentUrl = urlPart.join("/");
    //console.log("Url",currentUrl);
    //Se il ruolo di sistema è SUPERADMIN allora prosegue senza fare alcun controllo
    let currentUser: UserDTO = this.authService.getCurrentUser();

    // if (currentUser.SystemRole == SystemRole.ADMIN)
    //   return true;

    //Recupera il ruolo applicativo e, se non definito, restituisce un errore e blocca la navigazione
    let currentRole: RoleDTO = this.authService.getCurrentRole();

    if(!currentRole) {
      DialogService.Error("Non hai i pemessi per accedere a questa pagina!","permission-guard canActivate() 1");
      return false;
    }

    // Controllo se la URL corrente è contenuta almeno in una delle regole RoleRule
    // Continuo a cercare fino a che non trovo una regola. Se la trovo, allora ho i permessi.
    // Se alla fine del ciclo non ho trovato nulla non ho i permessi per accedere alla pagina.

    // SE siamo in modalità "Utente Tutorato" ignora le regole normali di RoleRules e prende solo quelle all'interno
    // della localStorage relativa all'utente tutorato

    var roleRules: Array<RoleRuleDTO>;
    roleRules = currentRole.RoleRules;
    // if (this.authService.getCurrentTutoredUser() == null) {
    // } else {

    //   roleRules = this.authService.getCurrentTutoredPermissions().RoleRules;
    // }

    console.log("permission.guard.ts", roleRules);

    roleRules.filter(x => x.RuleUrlRoutes != null).forEach((rr) => {
    //console.log("RoleRulkes",roleRules);

      if (allowed) return;
      var routes = rr.RuleUrlRoutes.split(";");

      // Debug: Mostra tutte le rotte per le quali si hanno i permessi
      //console.log("routes",routes);

      allowed = routes.indexOf(currentUrl) != -1;
      //if (allowed){ console.log(rr); }
    });

    if (!allowed) {
      DialogService.Error(this.translate.instant("Non hai i permessi per accedere a questa sezione!"),"permission-guard canActivate() 2");
      this.router.navigate(['/dashboard']);
      return false;
    }
  return true;

    

  }

}
