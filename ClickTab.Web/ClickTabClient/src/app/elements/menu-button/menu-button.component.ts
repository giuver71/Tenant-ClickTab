import { Component, Input } from '@angular/core';
import { MenuDTO } from '../../models/generics/menu.model';

@Component({
  selector: 'app-menu-button',
  templateUrl: './menu-button.component.html',
  styleUrl: './menu-button.component.scss'
})
export class MenuButtonComponent {
   @Input() menuItems: MenuDTO[] = [];
   
toggleSubmenu(item: MenuDTO) {
    item.expanded = !item.expanded;
  }
}
