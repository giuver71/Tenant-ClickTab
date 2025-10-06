import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';
import { Observable } from 'rxjs';


export interface ComponentCanDeactivate {
  canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
}

@Injectable()
export class PendingChangesGuard  {
  async canDeactivate(component: ComponentCanDeactivate, route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let result = component.canDeactivate();
    if (result == true)
      return true;
    else {
      const { value: confirmDeactivation } = await Swal.fire(
        {
          title: "Attenzione",
          text: "Vuoi uscire senza salvare i dati?",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonText: "Si",
          cancelButtonText: "No",
          allowOutsideClick: false,
          allowEscapeKey: false
        }).then((result) => {
          return result;
        })

      if (confirmDeactivation)
        return true;
      else
        return false;
    }
  }
}
