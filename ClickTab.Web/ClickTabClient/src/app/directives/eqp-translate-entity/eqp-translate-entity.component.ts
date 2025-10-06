import { OnInit, Component, Input } from '@angular/core';
import { TranslateEntityDTO } from '../../models/translateEntity.model';
import { LookupDTO } from '../../models/lookup.model';
import { TranslateService } from '@ngx-translate/core';
import { MultiLanguageService } from '../../services/multiLanguage.service';
import { DialogService } from '../../services/dialog.service';


@Component({
  selector: 'eqp-translate-entity',
  templateUrl: './eqp-translate-entity.component.html',
  styleUrls: ['./eqp-translate-entity.component.scss'],
})
export class EqpTranslateEntityComponent implements OnInit {

  /**
   * ID dell'entità per cui si vogliono gestire le traduzioni
   */
  @Input("EntityID") EntityID: number;

  /**
   * Tipo del modello server (Type espresso come stringa) dell'entità per cui si vogliono gestire le traduzioni
   */
  @Input("EntityType") EntityType: any;

  @Input("EntityLanguages") EntityLanguages: Array<LookupDTO> = null;

  translateEntity: TranslateEntityDTO = new TranslateEntityDTO();
  selectedLanguage: any;
  languageLabel: string = "";
  translationProperties: Array<string> = new Array<string>();

  constructor(public translate: TranslateService, private mlService: MultiLanguageService) {
    this.languageLabel = this.translate.instant("GENERIC_FIELDS.LANGUAGES");
  }

  ngOnInit() {
    //Verifica la presenza di entrambi i parametri altrimenti solleva un'eccezione
    if (!this.EntityID || this.EntityID == 0 || !this.EntityType || this.EntityType == "") {
      DialogService.Error("Per poter usare il componente eqp-translate-entity è obbligatorio indicare entrambi i parametri EntityID e EntityType");
      throw new Error("Per poter usare il componente eqp-translate-entity è obbligatorio indicare entrambi i parametri EntityID e EntityType");
    }
  }

  languageSelectionChanged(languageItem?: LookupDTO) {
    if (languageItem != undefined && languageItem != null)
      this.selectedLanguage = languageItem;
    if (this.selectedLanguage != null && this.selectedLanguage != undefined) {
      this.mlService.getEntityTranslation(this.EntityID, this.EntityType, this.selectedLanguage.ID)
        .then((res) => {
          this.translateEntity = res;
          if (this.translateEntity && this.translateEntity.TranslatedLabelFields) {
            this.translationProperties = Object.keys(this.translateEntity.TranslatedLabelFields);
          }
        })
        .catch((err) => {
          DialogService.Error(err.message);
        })
    }
  }

  /**
   * Salva la traduzione e, se richiesto, invoca l'evento di output per notificare il salvataggio al chiamante
   */
  saveTranslation() {

    //Verifica se ci sono campi obbligatori per i quali non è stato indicato il valore. Se ci sono allora mostra un messaggio d'errore altrimenti procede col salvataggio
    if (this.validateRequiredFields() != true) {
      DialogService.Error(this.translate.instant("DIALOGS.REQUIRED_FIELDS_NOT_SET"));
      return;
    }

    DialogService.Confirm(this.translate.instant("DIALOGS.CONFIRM_TRANSLATION"), () => {
      this.mlService.saveEntityTranslation(this.translateEntity)
        .then((res) => {
          DialogService.Success(this.translate.instant("DIALOGS.SUCCESS_SAVE_TRANSLATION"));
          if (this.mlService.translationEntitySaveResult)
            this.mlService.translationEntitySaveResult.emit(true);
        })
        .catch((err) => {
          DialogService.Error(err.message);
          if (this.mlService.translationEntitySaveResult)
            this.mlService.translationEntitySaveResult.emit(false);
        });
    });
  }

  validateRequiredFields(): boolean {
    let isValid: boolean = true;
    if (this.translateEntity.TranslatedDataRequired != null) {
      //Se esiste anche solo una proprietà obbligatoria per cui non è stato indicato il valore allora restituisce false
      this.translationProperties.forEach(prop => {
        if (this.translateEntity.TranslatedDataRequired[prop] == true && (this.translateEntity.RequestedTranslatedData[prop] == null || this.translateEntity.RequestedTranslatedData[prop] == undefined || this.translateEntity.RequestedTranslatedData[prop] == ""))
          isValid = false;
      });
    }

    return isValid;
  }
}
