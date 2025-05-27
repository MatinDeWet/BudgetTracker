import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AppAccountService } from '../../../services/AppAccountService';
import { TransactionListComponent } from "../../../components/transaction/transaction-list/transaction-list.component";

@Component({
  selector: 'app-account-detail',
  imports: [CommonModule, RouterLink, ReactiveFormsModule, TransactionListComponent],
  templateUrl: './account-detail.component.html',
  styleUrl: './account-detail.component.css'
})
export class AccountDetailComponent implements OnInit {
  accountService = inject(AppAccountService);
  route = inject(ActivatedRoute);
  router = inject(Router);

  accountId: string = '';
  account = signal<any>(null);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  isEditing = signal<boolean>(false);

  form = new FormGroup({
    name: new FormControl<string>('', { validators: [Validators.required, Validators.maxLength(64)]})
  });

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.accountId = id;
        this.loadAccountDetails();
      } else {
        this.errorMessage.set('Invalid account ID');
      }
    });
  }

  loadAccountDetails(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    
    this.accountService.getAccountById(this.accountId).subscribe({
      next: (data) => {
        this.account.set(data);
        this.form.patchValue({ name: data.name });
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load account details', err);
        this.errorMessage.set('Failed to load account details');
        this.isLoading.set(false);
      }
    });
  }
  
  toggleEditMode(): void {
    this.isEditing.update(value => !value);
    if (this.isEditing()) {
      this.form.patchValue({ name: this.account().name });
    }
  }
  
  saveAccount(): void {
    if (this.form.valid) {
      const accountName = this.form.value.name?.trim();
      if (accountName) {
        this.isLoading.set(true);
        this.accountService.updateAccount(this.accountId, accountName).subscribe({
          next: () => {
            this.isLoading.set(false);
            this.isEditing.set(false);
            this.loadAccountDetails();
          },
          error: (err) => {
            console.error('Failed to update account', err);
            this.errorMessage.set('Failed to update account');
            this.isLoading.set(false);
          }
        });
      }
    } else {
      this.form.markAllAsTouched();
    }
  }
  
  deleteAccount(): void {
    if (confirm(`Are you sure you want to delete the account "${this.account()?.name}"?`)) {
      this.accountService.deleteAccount(this.accountId).subscribe({
        next: () => {
          this.router.navigate(['/dashboard']);
        }
      });
    }
  }

  showAddTransactionModal(): void {
    this.router.navigate(['/accounts', this.accountId, 'transactions', 'new']);
  }

  handleTransactionDeleted(): void {
    this.loadAccountDetails();
  }
}
