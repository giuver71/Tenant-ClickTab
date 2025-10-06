import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FixTemplateService {

  /**
   * Fix sulle mat-tab, imposta tutte le scelte in overflow-hidden per permettere agli eqp-lookup di non essere tagliati e stare in posizione giusta
   * @param document Oggetto Document
   * @param renderer Oggetto Renderer2
   */

  putTabsOverflowHidden(document,renderer) {
    document.querySelectorAll("mat-tab-body.contains-lookup").forEach((elem) => {
      renderer.addClass(elem,"overflow-hidden");
    });
    setTimeout(function(){
      document.querySelectorAll("mat-tab-body.contains-lookup").forEach((elem2) => {
        renderer.removeClass(elem2,"overflow-hidden");
      });
    },500);
  }

  constructor() {

  }

  /**
   * Aggiusta l'header e la sidebar, viene chiamato anche in resize
   * @param event Oggetto Window
   * @param document Oggetto Document
   * @param renderer Oggetto Renderer2
   */

  public fixTemplate(event,document,renderer) : void {
    var self = this;
    if (!event.target){ event.target = event; }
    if (event.target.innerWidth < 992){
      renderer.addClass(document.body, 'brand-minimized');      
    }
    if (event.target.innerWidth >= 992){
      renderer.removeClass(document.body, 'brand-minimized');
    }
    
    setTimeout(function(){
      document.querySelectorAll(".sidebar .nav > .nav-dropdown > .nav-dropdown-items").forEach((elem) => {
        if (elem.querySelectorAll("a").length > 9 && document.body.classList.contains("sidebar-minimized")){
          renderer.setStyle(elem,"height","42vh");
          renderer.setStyle(elem,"overflow-y","auto");            
        } else {
          renderer.removeStyle(elem,"height");
          renderer.removeStyle(elem,"overflow-y");
        }
      });
    },50);
 
  }    

}
