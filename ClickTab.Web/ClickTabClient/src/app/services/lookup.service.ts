import { LookupDTO, LookupConfigDTO } from './../models/lookup.model';
import { environment } from './../../environments/environment';
import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ComplexLinqPredicateDTO } from '../models/linqPredicate.model';
import { LinqPredicateDTO } from '@eqproject/eqp-filters';

@Injectable({
  providedIn: 'root'
})
export class LookupService {

  /**
   * Variabile di appoggio che restituirà le informazioni alla chiusura della lookup in aggiunta (es: ID utente appena salvato)
   */
  lookupAddingComplete: EventEmitter<any> = new EventEmitter<any>();

  constructor(private http: HttpClient) {
  }

  /**
   * Data una stringa con il nome dell'entità, restituisce un array di LookupDTO dell'entità precedentemente passata
   * @param entityType Nome dell'entità
   */
  GetLookupEntities(entityType: string, filters: Array<LinqPredicateDTO> = null, complexFilters: Array<ComplexLinqPredicateDTO> = null): Promise<Array<LookupDTO>> {
    let config: LookupConfigDTO = new LookupConfigDTO();
    config.TypeName = entityType;
    config.Filters = filters;
    config.ComplexFilters = complexFilters;
    return this.http.post<Array<LookupDTO>>(environment.apiFullUrl + '/lookup/GetLookupEntities', config).toPromise()
  }
}
