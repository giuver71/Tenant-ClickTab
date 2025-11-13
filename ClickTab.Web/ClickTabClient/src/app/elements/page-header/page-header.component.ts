import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';



@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss']
})
export class PageHeaderComponent {
  /** Titolo mostrato nel componente */
  @Input() title = '';

  /** Array di pulsanti azione */
  @Input() buttons: HeaderButton[] = [];

  /** Rotta opzionale per override del goBack */
  @Input() backRoute?: string;

  constructor(private location: Location, private router: Router) {}

  goBack(): void {
    if (this.backRoute) {
      this.router.navigateByUrl(this.backRoute);
    } else {
      this.location.back();
    }
  }

  handleAction(action: () => void): void {
    if (action) action();
  }
}

export class HeaderButton {
  icon!: string;
  label!: string;
  isDisabled?:boolean=false;
  action!: () => void;
  color?: string; // es. 'btn-primary', 'btn-secondary', 'btn-outline-dark'
}