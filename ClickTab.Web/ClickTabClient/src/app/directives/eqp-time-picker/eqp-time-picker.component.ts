import { BehaviorSubject } from 'rxjs';
import { Component, OnInit, Input, Output, forwardRef, EventEmitter, ChangeDetectionStrategy, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'eqp-time-picker',
    templateUrl: './eqp-time-picker.component.html',
    styleUrls: ['./eqp-time-picker.component.scss'],
      providers: [
        {
          provide: NG_VALUE_ACCESSOR,
          useExisting: forwardRef(() => EqpTimePickerComponent),
          multi: true
        },
      ],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class EqpTimePickerComponent implements OnInit, AfterViewInit, ControlValueAccessor {

    onChange: any = () => { }
    onTouch: any = () => { }
    val: any = null;

    /**
     * Label che sarà visibile come etichetta della lookup
     */
    @Input("placeholder") placeholder: any;

    /**
     * Definisce se la direttiva è solo in lettura
     */
    @Input("isReadonly") isReadonly: boolean = false;

    /**
     * Definisce se la direttiva è obbligatoria
     */
    @Input("isRequired") isRequired: boolean = false;

    @Input("isDisabled") isDisabled: boolean = false;

    /**
       * Scrivere in caso di utilizzo di formControlName, il nome del formGroup utilizzato nel tag <form>
       */
    @Input("formGroupInput") formGroupInput: any;

    /**
       * Nome del formControlName da utilizzare
       */
    @Input("formControlNameInput") formControlNameInput: any;

    /**
   * ngModel da bindare
   */
    @Input("ngModelInput") ngModelInput: any;

    /**
   * formato da utilizzare, di default è impostato su 12 in modo da mostrare AM e PM.
   * Se invece si passa 24 allora le ore andranno da 00:00 a 23:29
   */
     @Input("format") format: number = 12;

     /**   
      * Permette di definire l'orario minimo da poter selezionare
      */
     @Input("min") min: any = null;

    /**
     * Evento scatenato al cambiare del valore della select
     */
    @Output() ngModelInputChange: EventEmitter<any> = new EventEmitter<any>();

    private _data = new BehaviorSubject<any[]>([]);

    pickers: any[] = [Math.random().toString(36).substring(7)];

    // change data to use getter and setter
    @Input()
    set data(value) {
        // set the latest value for _data BehaviorSubject
        this._data.next(value);
    };

    get data() {
        // get the latest value from _data BehaviorSubject
        return this._data.getValue();
    }

    constructor(private cd: ChangeDetectorRef) { 
    }

    ngOnInit(): void {
        //Se obbligatoria aggiunge l'asterisco al placeholder
        if (this.isRequired == true)
            this.placeholder += " *";
    }

    ngAfterViewInit() {
        this.cd.detectChanges();
    }

    /**
     * Evento scatenato alla selezione dell'ora (quando si clicca sul pulsante di conferma).
     * Si occupa di sovrascrivere l'ngModelInput con il valore impostato nel time-picker
     * @param $event 
     */
    timePickerSelectionChanged($event) {
        this.ngModelInput = $event;
        if (this.ngModelInputChange != null) {
            this.ngModelInputChange.emit($event);
            this.value = $event;
          }
    }

    //#region  Sezione two way bindings: Necessario per il funzionamento corretto del ControlValueAccessor (binding ngModel tra più componenti)
    set value(val) {
        this.val = val
        this.onChange(val)
        this.onTouch(val)
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
