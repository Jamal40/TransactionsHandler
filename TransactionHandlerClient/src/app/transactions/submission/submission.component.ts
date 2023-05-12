import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TransactionsService } from 'src/app/Services/transactions.service';
import ProcessTransactionRequest from 'src/app/types/ProcessTransactionRequest';
import { ResultModalComponent } from '../result-modal/result-modal.component';
import ProcessTransactionResponse from 'src/app/types/ProcessTransactionResponse';

@Component({
  selector: 'app-submission',
  templateUrl: './submission.component.html',
  styleUrls: ['./submission.component.css'],
})
export class SubmissionComponent {
  constructor(
    private readonly transactionService: TransactionsService,
    private readonly router: Router,
    private readonly dialog: MatDialog
  ) {}

  form = new FormGroup({
    processingCode: new FormControl<string>('999000'),
    systemTraceNr: new FormControl<string>('36'),
    functionCode: new FormControl<string>('1324'),
    cardNo: new FormControl<string>('4712345601012222'),
    cardHolder: new FormControl<string>('Ahmed'),
    amountTrxn: new FormControl<string>('1000'),
    currencyCode: new FormControl<string>('840'),
  });

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.form.invalid) return;
    var request = new ProcessTransactionRequest();
    request.processingCode = this.form.get('processingCode')?.value ?? '';
    request.systemTraceNr = this.form.get('systemTraceNr')?.value ?? '';
    request.functionCode = this.form.get('functionCode')?.value ?? '';
    request.cardNo = this.form.get('cardNo')?.value ?? '';
    request.cardHolder = this.form.get('cardHolder')?.value ?? '';
    request.amountTrxn = this.form.get('amountTrxn')?.value ?? '';
    request.currencyCode = this.form.get('currencyCode')?.value ?? '';

    this.transactionService.submitTransaction(request).subscribe({
      next: (response) => {
        this.openDialog(response);
      },
      error: () => {
        alert('Something went wrong');
      },
    });
  }

  openDialog(data: ProcessTransactionResponse): void {
    const dialogRef = this.dialog.open(ResultModalComponent, {
      data,
    });
  }
}
