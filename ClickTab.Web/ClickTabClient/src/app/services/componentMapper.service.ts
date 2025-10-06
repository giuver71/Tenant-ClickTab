import { Injectable } from '@angular/core';
import { COMPONENT_MAPPER } from '../../modules/shared.module';

@Injectable({
  providedIn: 'root'
})


export class ComponentMapperService {

  constructor() { }

  static register(key: string, value: any) {
    COMPONENT_MAPPER.pool[key] = value;
  }

}

