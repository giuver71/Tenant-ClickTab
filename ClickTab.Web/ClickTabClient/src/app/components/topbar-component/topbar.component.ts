import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { INavData } from '@eqproject/eqp-ui';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopbarComponent implements OnInit {
  
  @Input() navItems!:INavData[];
  @Input() activeItem: any;
  @Output() itemSelected = new EventEmitter<any>();

   openMenus = new Set<INavData>();

  ngOnInit(): void {
    console.log("NavItems",this.navItems);
  }

  onSelect(item: any) {
    if (event) event.stopPropagation();
    this.itemSelected.emit(item);
  }

  isActive(item: any): boolean {
    return this.activeItem && this.activeItem.id === item.id;
  }
showChildren(item: INavData) {
    this.openMenus.add(item);
  }

  hideChildren(item: INavData) {
    this.openMenus.delete(item);
  }
  isOpen(item: INavData): boolean {
    return this.openMenus.has(item);
  }
}
