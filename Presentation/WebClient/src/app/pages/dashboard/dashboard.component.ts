import { Component, inject, OnInit, signal } from '@angular/core';
import { AppAccountService } from '../../services/AppAccountService';
import { SearchAccountResponse } from '../../api/generation/models';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule, RouterLink, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent{
  accountService = inject(AppAccountService);
  showCreateForm = signal<boolean>(false);

  form = new FormGroup({
    name : new FormControl<string>('', { validators: [Validators.required, Validators.maxLength(64)]})
  });
  
  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.accountService.searchAccounts().subscribe();
  }

  toggleCreateForm(): void {
    this.showCreateForm.update(value => !value);
  }

  createAccount(): void {
    if (this.form.valid) {
      const accountName = this.form.value.name?.trim();
      if (accountName) {
        this.accountService.createAccount(accountName).subscribe({
          next: () => {
            this.form.reset();
            this.showCreateForm.set(false);
          }
        });
      }
    } else {
      this.form.markAllAsTouched();
    }
  }

  deleteAccount(account: SearchAccountResponse): void {
    if (confirm(`Are you sure you want to delete the account "${account.name}"?`)) {
      this.accountService.deleteAccount(account.id!).subscribe();
    }
  }
}
