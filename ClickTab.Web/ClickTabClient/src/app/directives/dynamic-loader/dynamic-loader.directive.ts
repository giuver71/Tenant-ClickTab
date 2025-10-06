import { Directive, Input, ViewContainerRef } from '@angular/core';
import { COMPONENT_MAPPER } from '../../../modules/shared.module';

@Directive({
  selector: '[appDynamicLoader]'
})

export class DynamicLoaderDirective {

  // In input verrà passato il selettore del componente da ricreare
  @Input() set data(data: DynamicLoaderDirectiveData) {
    this.load(data.componentSelectorName, data.inputParams);
  }

  constructor(private viewContainerRef: ViewContainerRef) { }

  load(componentSelector: string, inputParams: any = null): any {

    if (!componentSelector) {
      throw new Error('Failed to load dynamic component');
    }

    let componentInstance = this.getComponent(componentSelector);


    const viewContainerRef = this.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent(componentInstance);

    //Se sono stati passati dei parametri di input allora li memorizza nel componente caricato dinamicamente.
    //E' ovvio che il nome dei parametri di input passati al DynamicLoader deve coincidere con il nome del parametro di input passato al componente caricato dinamicamente
    if (inputParams != null) {
      let inputProperties = Object.keys(inputParams);
      inputProperties.forEach(p => {
        componentRef.instance[p] = inputParams[p];
      });
    }
  }

  isIInjectLookup(object: any) {
    return 'DisableRedirectAfterSave' in object;
  }

  /**
* Restrituisce un componente passandogli come parametro la chiave
* @param key
*/
  getComponent(key: any) {
    let component = key;
    if (typeof key == "string") {
      component = COMPONENT_MAPPER.get(key);
    }
    return component;
  }
}

export class DynamicLoaderDirectiveData {
  componentSelectorName: string;
  inputParams?: any;
}
