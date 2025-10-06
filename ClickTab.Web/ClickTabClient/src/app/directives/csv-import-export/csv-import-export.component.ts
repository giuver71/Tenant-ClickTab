import { ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DialogService } from '../../services/dialog.service';
import { ImportExportService } from '../../services/importExport.service';

@Component({
  selector: 'csv-import-export',
  templateUrl: './csv-import-export.component.html',
  styleUrls: ['./csv-import-export.component.scss']
})
export class CsvImportExportComponent implements OnInit {

  /**
   * Permette di definire input aggiuntivi da usare per l'importazione (es: il type per l'entità archive che dipende dalla voce di menù)
   */
  @Input("additionalParams") additionalParams: any = null;

  @Input("isDisabled") isDisabled: boolean = false;
  @Input("showRaisedButton") showRaisedButton: boolean = false;

  /**
   * Permette di definire il nome del type dell'entità interessata dalla procedura di export/import
   */
  @Input("entityTypeName") entityTypeName: string;
  @Input("importLabel") importLabel: string = "Importa tracciato excel";
  @Input("exportLabel") exportLabel: string = "Esporta tracciato excel";
  @Input("mainLabel") mainLabel: string ="Importa da tracciato";
  @Input("mainTooltip") mainTooltip: string;

  /**
   * Evento invocato al completamento della procedura di import
   */
  @Output("importCompleted") importCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();


  @ViewChild('CSVfileInput', { static: false }) CSVFileInput: ElementRef;

  fileToImport: Blob;
  url;
  fileToUpload: File = null;

  constructor(private importExportService: ImportExportService, private translate: TranslateService) { }

  ngOnInit(): void {
    if (this.entityTypeName == null || this.entityTypeName == "") {
      DialogService.Error("Il parametro 'entityTypeName' nel componente 'csv-import-export' è obbligatorio");
      throw new Error("Il parametro 'entityTypeName' nel componente 'csv-import-export' è obbligatorio");
    }
  }

  csvDownload() {
    var self = this;
    this.importExportService.exportTracciato(this.entityTypeName).then((res) => {
      this.downLoadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", self.entityTypeName + ".xlsx");
    })
      .catch((err) => {
        DialogService.Error(err.message);
      });
  }

  csvImport(event) {
    if (event.target.files && event.target.files[0]) {
      this.fileToUpload = event.target.files[0];
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.url = event.target.result;
        this.fileToImport = this.convertBase64ToBlob(this.url);
        this.importExportService.importTracciato(this.entityTypeName, this.fileToImport, this.additionalParams).then((res) => {
          DialogService.Success("Operazione completata");
          if(this.importCompleted)
            this.importCompleted.emit(true);

            //Se alcuni record non sono stati importati per degli errori nei
            //dati allora è presente il base64 contenente il file xlsx con i record non importati,
            //quindi in quel caso scarica il file
            if(res.HasErrors == true && res.FileBase64) {
              DialogService.Warning("L'operazione è stata completata ma ci sono alcuni record che non è stato possibile importare");
              let a = document.createElement("a");
              a.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + res.FileBase64;
              a.download = this.entityTypeName + " - Import Errors.xlsx";
              a.click();
            }
        })
          .catch((err) => {
            DialogService.Error(err.message);
            if(this.importCompleted)
              this.importCompleted.emit(false);
          })
          .finally(() => {
            this.resetFileInput();
          });
      }

    }
  }

  resetFileInput() {
    if (this.CSVFileInput)
      this.CSVFileInput.nativeElement.value = "";

    this.fileToImport = null;
  }

  convertBase64ToBlob(base64Image: string) {
    // Split into two parts
    const parts = base64Image.split(';base64,');

    // Hold the content type
    const imageType = parts[0].split(':')[1];

    // Decode Base64 string
    const decodedData = window.atob(parts[1]);

    // Create UNIT8ARRAY of size same as row data length
    const uInt8Array = new Uint8Array(decodedData.length);

    // Insert all character code into uInt8Array
    for (let i = 0; i < decodedData.length; ++i) {
      uInt8Array[i] = decodedData.charCodeAt(i);
    }

    // Return BLOB image after conversion
    return new Blob([uInt8Array], { type: imageType });
  }

  downLoadFile(data: any, type: string, fileName: string) {
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.style.display = 'none';

    let blob = new Blob([data.body], { type: type });
    let url = window.URL.createObjectURL(blob);

    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);

  }
}
