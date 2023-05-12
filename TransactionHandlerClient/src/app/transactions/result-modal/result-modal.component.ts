import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import ProcessTransactionResponse from 'src/app/types/ProcessTransactionResponse';

@Component({
  selector: 'app-result-modal',
  templateUrl: './result-modal.component.html',
  styleUrls: ['./result-modal.component.css'],
})
export class ResultModalComponent {
  constructor(
    public dialogRef: MatDialogRef<ResultModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProcessTransactionResponse,
    private readonly router: Router
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
