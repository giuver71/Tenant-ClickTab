import { Injectable } from "@angular/core";
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })

/**
 * Servizio per gestire lo spinner
 */
export class SpinnerService {
  isLoading = new Subject<boolean>();

  private requestsCount: number = 0;

  /**
   * Mostra lo spinner
   */
  show() {
    setTimeout(() => {
      this.isLoading.next(true);
    }, 100);
  }

  /**
   * Nasconde lo spinner
   */
  hide() {
    setTimeout(() => {
      this.isLoading.next(false);
    }, 100);
  }

  addRequestCounter() {
    this.requestsCount++;
    this.show();
  }

  removeRequestCounter() {
    if (this.requestsCount > 0)
      this.requestsCount--;
    if (this.requestsCount == 0)
      this.hide();
  }
}