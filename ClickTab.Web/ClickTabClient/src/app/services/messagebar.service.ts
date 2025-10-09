import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MessageBarComponent } from '../elements/message-bar/message-bar-component';

export type MessageBarType = 'success' | 'error' | 'warning' | 'info';

export interface MessageBarOptions {
  message: string;
  duration?: number;        // millisecondi (es. 5000)
  actionLabel?: string;     // testo del bottone (es. "Chiudi")
  type?: MessageBarType;    // tipo del messaggio
  onClose?: () => void;     // callback eseguita alla chiusura
}

@Injectable({ providedIn: 'root' })
export class MessageBarService {
  constructor(private snackBar: MatSnackBar) {}

 show(options: MessageBarOptions): void {
  const ref = this.snackBar.openFromComponent(MessageBarComponent, {
    data: options,
    duration: options.duration || undefined,
    horizontalPosition: 'center',
    verticalPosition: 'top', // posizione di base
    panelClass: ['message-bar-panel', 'centered-snackbar']
  });

    ref.afterDismissed().subscribe(() => {
      if (options.onClose) options.onClose();
    });
  }
}
