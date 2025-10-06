import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'body',
  template: '<app-spinner></app-spinner><router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {
  title = 'Title';

  constructor(
    private router: Router,
    private titleService: Title,
  ) {
    titleService.setTitle(this.title);
  }

  ngOnInit(): void {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
    });
  }
}
