import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MessageBarOptions } from '../../services/messagebar.service';

@Component({
  selector: 'app-message-bar',
  templateUrl: './message-bar.component.html',
  styleUrls: ['./message-bar.component.scss']
})
export class MessageBarComponent {
  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: MessageBarOptions,
    private snackBarRef: MatSnackBarRef<MessageBarComponent>
  ) {}

  close(): void {
    this.snackBarRef.dismiss();
  }

  get icon(): string {
    switch (this.data.type) {
      case 'success': return 'fa fa-check-circle';
      case 'error':   return 'fa fa-times-circle';
      case 'warning': return 'fa fa-exclamation-triangle';
      default:        return 'fa fa-info-circle';
    }
  }

  get colorClass(): string {
    return this.data.type ? `msg-${this.data.type}` : 'msg-info';
  }
}
