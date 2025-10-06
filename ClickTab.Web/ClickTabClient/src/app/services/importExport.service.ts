import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CsvImportResultDTO } from '../models/generics/csvImportResult.model';


@Injectable({
  providedIn: 'root'
})

/**
 * Servizio di esportazione tracciato e importazione csv
 */
export class ImportExportService {

  constructor(private http: HttpClient) { }

  /**
   * Dato il nome del type dell'entità si occupa di richiamare l'API che restituisce il tracciato XLSX
   * con tutte le colonne relative all'entità richiesta
   * @param entityTypeName
   */
  exportTracciato(entityTypeName: string) {
    return this.http.get(environment.apiFullUrl + '/ImportExport/getCSVTrack/' + entityTypeName, {
      responseType: 'blob',
      observe: 'response'
    }).toPromise();
  }

  /**
   * Dato il nome del type di una specifica entità e il file xlsx contenente i dati degli elementi da importare
   * si occupa di richiamare l'API che processa il file e importa i dati sul DB, eseguendo anche tutti i controlli di validazione.
   * Se alcune righe del file excel non possono essere importate per problemi di formattazione allora l'API restituisce un nuovo file XLSX
   * con evidenziate le celle che contengono errori.
   * @param entityTypeName
   * @param file
   */
  importTracciato(entityTypeName: string, fileToImport: Blob, additionalParams: any = null): Promise<CsvImportResultDTO> {
    const formData = new FormData();
    formData.append('file', fileToImport);

    if (additionalParams == null || additionalParams == undefined) {
      additionalParams = {};
    }

    formData.append('additionalParams', JSON.stringify(additionalParams));

    return this.http.post<CsvImportResultDTO>(environment.apiFullUrl + '/ImportExport/ImportCsv/' + entityTypeName, formData).toPromise();
  }

}
