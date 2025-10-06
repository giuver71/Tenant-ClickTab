import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, OnChanges, SimpleChanges, ChangeDetectorRef, AfterViewChecked } from "@angular/core";
import { TranslateService } from '@ngx-translate/core';
import { ControlValueAccessor } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { KeyValue } from '@angular/common';
import { FloatLabelType } from "@angular/material/form-field";

@Component({
  selector: 'eqp-ml-field',
  templateUrl: './eqp-multilanguage-field.component.html',
  styleUrls: ['./eqp-multilanguage-field.component.scss'],
})
export class EqpMultiLanguageFieldComponent implements ControlValueAccessor, OnChanges {

  onChange: any = () => { }
  onTouch: any = () => { }
  val: any = null;

  //#region Input Direttiva

  /**
  * Proprietà da bindare all'ngModel
  */
  @Input("ngModelInput") ngModelInput: any;

  /**
   * Nome del formControlName da utilizzare
   */
  @Input("formControlNameInput") formControlNameInput: any;

  /**
 * Scrivere in caso di utilizzo di formControlName, il nome del formGroup utilizzato nel tag <form>
 */
  @Input("formGroupInput") formGroupInput: any;

  /**
   * Definisce se la casella di testo è solo in lettura
   */
  @Input("isReadonly") isReadonly: boolean = false;

  /**
   * Definisce se la casella di testo è obbligatoria
   */
  @Input("isRequired") isRequired: boolean = false;

  /**
   * Definisce se la casella di testo è obbligatoria
   */
  @Input("isDisabled") isDisabled: boolean = false;

  /**
  * Placeholder da visualizzare
  */
  @Input("placeholder") placeholder: string;

  /**
   * Label che sarà visibile come etichetta della lookup
   */
  @Input("labelTranslateKey") labelTranslateKey: string;

  /**
   * Permette di definire l'ID della lingua con cui il dato è stato inserito di default
   */
  @Input("fkDefaultLanguage") fkDefaultLanguage: number;

  /**
   * Permette di definire il dizionario dei valori inseriti nella lingua di default (il valore è opzionale)
   */
  @Input("defaultTranslatedData") defaultTranslatedData: any = null;

  /**
   * Permette di definire il dizionario dei valori inseriti nella lingua di default (il valore è opzionale)
   */
  @Input("defaultTranslatedProperty") defaultTranslatedProperty: string = null;

  /**
   * Definisce l'input di testo deve essere una text-area oppure un semplice input text (default = input text)
   */
  @Input("isTextArea") isTextArea: boolean = false;

  mlPlaceholder: string = "";
  mlPlaceholderType: FloatLabelType = "auto";

  //#endregion

  //#region Output direttiva

  @Output() ngModelInputChange: EventEmitter<string> = new EventEmitter<string>();

  //#endregion

  constructor(public translate: TranslateService, private authService: AuthService, private readonly cd: ChangeDetectorRef) {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['fkDefaultLanguage'] != undefined && changes['defaultTranslatedData'] != undefined) {
      this.fkDefaultLanguage = changes.fkDefaultLanguage.currentValue;
      this.defaultTranslatedData = changes.defaultTranslatedData.currentValue;
      this.setupMultiLanguagePlaceholder();
    }
  }

  setupMultiLanguagePlaceholder() {
    //1) Se esiste una lingua di default con cui il valore risulta inserito per la prima volta
    //2) Se tale lingua è diversa da quella con cui è stato effettuato il login
    //3) Se l'ngModel risulta popolato
    //Allora copia l'ngModel nel placeholder e svuota la proprietà
    if (this.fkDefaultLanguage != undefined
      && this.fkDefaultLanguage != null
      && this.authService.getCurrentLanguage().ID != this.fkDefaultLanguage
      && (this.ngModelInput == null || this.ngModelInput == undefined)) {
      this.mlPlaceholderType = "always";
      this.mlPlaceholder = this.defaultTranslatedData[this.defaultTranslatedProperty];
    }

    this.cd.detectChanges();
  }

  inputChanged(event) {
    if (this.ngModelInputChange != null) {
      this.ngModelInputChange.emit(event);
      this.value = this.ngModelInput;
    }
  }

  //#region Sezione two way bindings: Necessario per il funzionamento corretto del ControlValueAccessor (binding ngModel tra più componenti)
  set value(val) {
    this.val = val

    this.onChange(val)
    this.onTouch(val)

    this.ngModelInputChange.emit(this.val);
  }

  writeValue(value: any) {
    this.value = value
  }

  registerOnChange(fn: any) {
    this.onChange = fn
  }

  registerOnTouched(fn: any) {
    this.onTouch = fn
  }

  //#endregion
}
