import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AppTransactionService } from '../../../services/AppTransacitonService';

@Component({
  selector: 'app-transaction-form',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './transaction-form.component.html',
  styleUrl: './transaction-form.component.css'
})
export class TransactionFormComponent implements OnInit {
transactionService = inject(AppTransactionService);
  route = inject(ActivatedRoute);
  router = inject(Router);

  accountId: string = '';
  transactionId: string | null = null;
  isEditing: boolean = false;
  isLoading: boolean = false;
  errorMessage: string | null = null;

  form = new FormGroup({
    description: new FormControl<string>('', { validators: [Validators.required, Validators.maxLength(200)] }),
    amount: new FormControl<number>(0, { validators: [Validators.required] }),
    date: new FormControl<string>(this.getCurrentDate(), { validators: [Validators.required] })
  });

  ngOnInit(): void {
    this.accountId = this.route.snapshot.paramMap.get('accountId') || '';
    this.transactionId = this.route.snapshot.paramMap.get('id');
    this.isEditing = !!this.transactionId;

    if (this.isEditing && this.transactionId) {
      this.loadTransactionDetails(this.transactionId);
    }
  }

  getCurrentDate(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  loadTransactionDetails(id: string): void {
    this.isLoading = true;
    this.transactionService.getTransactionById(id).subscribe({
      next: (data) => {
        // Convert date to YYYY-MM-DD format for the input field
        const dateObj = new Date(data.date!);
        const formattedDate = dateObj.toISOString().split('T')[0];
        
        this.form.patchValue({
          description: data.description,
          amount: data.amount,
          date: formattedDate // Use the formatted date
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load transaction details', err);
        this.errorMessage = 'Failed to load transaction details';
        this.isLoading = false;
      }
    });
    }

  onSubmit(): void {
    if (this.form.valid) {
      const { description, amount, date } = this.form.value;
      
      if (description && amount !== undefined && date) {
        this.isLoading = true;
        
        // Convert date to UTC format
        const dateUTC = this.convertToUTCFormat(date);
        
        if (this.isEditing && this.transactionId) {
          // Update existing transaction
          this.transactionService.updateTransaction(
            this.transactionId,
            amount!,
            description,
            dateUTC
          ).subscribe({
            next: () => {
              this.router.navigate(['/accounts', this.accountId]);
            },
            error: (err) => {
              console.error('Failed to update transaction', err);
              this.errorMessage = 'Failed to update transaction';
              this.isLoading = false;
            }
          });
        } else {
          // Create new transaction
          this.transactionService.createTransaction(
            this.accountId,
            amount!,
            description,
            dateUTC
          ).subscribe({
            next: () => {
              this.router.navigate(['/accounts', this.accountId]);
            },
            error: (err) => {
              console.error('Failed to create transaction', err);
              this.errorMessage = 'Failed to create transaction';
              this.isLoading = false;
            }
          });
        }
      }
    } else {
      this.form.markAllAsTouched();
    }
  }

  convertToUTCFormat(dateString: string): string {
    const date = new Date(dateString);
    date.setUTCHours(0, 0, 0, 0);
    return date.toISOString();
  }
}
