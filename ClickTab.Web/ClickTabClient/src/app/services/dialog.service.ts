import { ToastrService } from "ngx-toastr";
import Swal, { SweetAlertInput } from 'sweetalert2';
import { InjectorInstance } from "../app.module";

const toastOptions: any = {
  timeOut: 5000,
  positionClass: 'toast-bottom-right',
  progressBar: true
}

// @Injectable({ providedIn: 'root' })

/**
 * Servizio per i messaggi di dialogo
 */
export class DialogService {

  static toastrService: ToastrService = null;

  /**
   * Utilizza l'InjectorInstance definita nell'app.module per iniettare i servizi che sono necessari in questa classe.
   * Il metodo è stato necessario perchè questo servizio espone metodi statici quindi non sarebbe stato possibile usare l'inject con il costruttore della classe
   */
  static injectServices() {
    this.toastrService = InjectorInstance.get<ToastrService>(ToastrService);
  }

  /**
   * Mostra un messaggio di tipo SUCCESS come popup sweetalert (o come notifica toast se isToast = true)
   * @param message Messaggio da mostrare
   * @param title Titolo del messaggio (di default mostra 'Operazione completata')
   * @param isToast Se TRUE mostra la notifica toast invece che uno sweetalert
   */
  static Success(message: string, title: string = null, isToast: boolean = false) {
    let currentTitle = title != null ? title : 'Operazione completata con successo.';
    if (isToast == null || isToast != true)
      return Swal.fire(currentTitle, message, 'success');
    else {
      this.injectServices();
      this.toastrService.success(message, currentTitle, toastOptions);
    }
  }

  /**
   * Mostra un messaggio di tipo ERROR come popup sweetalert (o come notifica toast se isToast = true)
   * @param message Messaggio d'errore da mostrare
   * @param title Titolo del messaggio (di default mostra 'Errore')
   * @param isToast Se TRUE mostra la notifica toast invece che uno sweetalert
   */
  static Error(message: string | string[], title: string = null, isToast: boolean = false) {
    let currentTitle = title != null ? title : 'Errore';

    //Se è stato passato un elenco di messaggi allora crea un html concatenando i diversi messaggi
    //col ritorno a capo
    if (Array.isArray(message)) {
      message = message.join("<br>");
    }

    if (isToast == true) {
      this.injectServices();
      this.toastrService.error(message, title, toastOptions);
    }
    else {
      return Swal.fire({
        title: currentTitle,
        html: message,
        icon: 'error'
      });
    }
  }

  /**
   * Mostra un messaggio di tipo INFO come popup sweetalert (o come notifica toast se isToast = true)
   * @param message Messaggio da mostrare
   * @param title Titolo del messaggio (di default mostra 'Info')
   * @param isToast Se TRUE mostra la notifica toast invece che uno sweetalert
   */
  static Info(message: string, title: string = null, isToast: boolean = false) {
    let currentTitle = title != null ? title : 'Info';
    if (isToast == true) {
      this.injectServices();
      this.toastrService.info(message, title, toastOptions);
    }
    else {
      return Swal.fire(currentTitle, message, 'info');
    }
  }

  /**
   * Mostra uno sweetalert di tipo WARNING con il messaggio passato come parametro
   * @param message Messaggio da mostrare nello sweetalert
   * @param title Titolo dello sweetalert (di default mostra 'Attenzione!')
   */
  static Warning(message: string | string[], title: string = null, isToast: boolean = false) {
    let currentTitle = title != null ? title : 'Attenzione';

    //Se è stato passato un elenco di messaggi allora crea un html concatenando i diversi messaggi
    //col ritorno a capo
    if (Array.isArray(message)) {
      message = message.join("<br>");
    }

    if (isToast == true) {
      this.injectServices();
      this.toastrService.warning(message, title, toastOptions);
    }
    else {
      return Swal.fire({
        title: currentTitle,
        html: message,
        icon: 'warning'
      });
    }
  }

  /**
   * Mostra uno sweetalert di tipo CONFIRM con il messaggio passato come parametro.
   * Alla pressione del tasto CONFERMA viene invocata la funzione confirmCallback.
   * Se viene passata anche la funzione cancellCallback viene invocata quando viene premuto il tasto ANNULLA
   * @param message Messaggio da mostrare nello sweetalert (può contenere una lista di messaggi)
   * @param confirmCallback Funzione di callback da invocare quando viene premuto il tasto CONFERMA
   * @param isWarning Se TRUE allora utilizza l'icona di WARNING all'interno dello sweetalert
   * @param title Titolo dello sweetalert (di default mostra 'Sei sicuro di voler procedere?')
   * @param customWidth Larghezza da impostare per lo sweetalert (default: 32rem)
   * @param cancelCallback Funzione di callback da invocare alla pressione del tasto ANNULLA (opzionale)
   * @param confirmButtonText Testo da mostrare nel pulsante di conferma (default: Conferma)
   * @param cancelButtonText Testo da mostrare nel pulsante di annulla (default: Chiudi)
   */
  static Confirm(message: string | string[], confirmCallback: any, isWarning: boolean = false,
    title: string = null, customWidth: string = null, cancelCallback: any = null, confirmButtonText: string = null, cancelButtonText: string = null) {

    let currentTitle = title != null ? title : 'Sei sicuro di voler procedere?';
    if (Array.isArray(message)) {
      message = message.join("<br>");
    }

    Swal.fire({
      title: currentTitle,
      html: message,
      width: customWidth ? customWidth : '32rem',
      icon: !isWarning ? 'question' : 'warning',
      confirmButtonText: confirmButtonText ? confirmButtonText : 'Conferma',
      showCancelButton: true,
      cancelButtonText: cancelButtonText ? cancelButtonText : 'Chiudi',
      allowOutsideClick: false,
      allowEscapeKey: false
    }).then((result) => {
      if (result.value && confirmCallback) {
        confirmCallback();
      } else if (cancelCallback) {
        cancelCallback();
      }
    });
  }

  /**
   * Aprte uno sweetalert con un input.
   * @param message Messaggio da mostrare nel prompt
   * @param confirmCallback Funzione di callback da richiamare sul conferma
   * @param inputType Definisce il tipo di input da visualizzare scelto tra quelli messi a disposizione dal componente
   * @param inputOptions Se "inputType" è "radio" o "select" qui vanno specificate le opzioni per la selezione
   * @param inputValidator Funzione per validare l'input mostrato, prende in ingresso il result inserito dall'utente
   */
  static Prompt(message: string, confirmCallback: any, showCancelButton: boolean = true, inputType: SweetAlertInput = 'text', inputPlaceholder: string = "", inputOptions: Record<string, any> = {}, inputValidator: any = undefined) {
    Swal.fire({
      title: message,
      input: inputType,
      inputPlaceholder: inputPlaceholder,
      inputAttributes: {
        autocapitalize: 'off'
      },
      inputOptions: inputOptions,
      inputValidator: (value: any): Promise<string> => {
        return new Promise((resolve) => {
          resolve(inputValidator(value))
        })
      },
      showCancelButton: showCancelButton,
      confirmButtonText: 'Conferma',
      cancelButtonText: 'Esci',
      showLoaderOnConfirm: true,
      allowOutsideClick: false
    }).then((result) => {
      if (result.value && confirmCallback) {
        confirmCallback(result.value);
      }
    })
  }

  /**
   * Mostra un toast message che annuncia la ricezione di una nuova notifica.
   * Se passata un action da eseguire (es. aprire il dialog della notifica ricevuta) viene eseguita
   * se l'utente clicca sulla notifica e non se la chiude
   * @param message Messaggio da mostrare nel toast (default: 'Hai ricevuto una nuova notifica')
   * @param currentTitle Titolo del toast (default: 'Hai 1 una nuova notifica')
   * @param closeToastAction Funzione di callback(opzionale) da passare per gestire l'eventuale click sul toast. IN tal caso quando viene cliccato il toast viene invocata la funzione passata
   */
  static ShowNewNotification(message: string = null, currentTitle: string = null, closeToastAction: any = null) {
    let firedToast: any;

    this.injectServices();

    if (!message)
      message = "Hai ricevuto una nuova notifica";
    if (!currentTitle)
      currentTitle = "Hai 1 nuova notifica";

    const _toastOptions: any = {
      disableTimeOut: true,
      positionClass: 'toast-bottom-right',
      toastClass: 'ngx-toastr notification-toastr',
      closeButton: true
    }

    firedToast = this.toastrService.info(message, currentTitle, _toastOptions);
    if (closeToastAction)
      firedToast.onTap.subscribe((close) => { closeToastAction(); });
  }
}
