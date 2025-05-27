import { Injectable, computed, effect, inject, signal } from '@angular/core';
import { AccountService } from '../api/generation/services';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { GetAccountByIdResponse } from '../api/generation/models/get-account-by-id-response';
import { CreateAccountResponse } from '../api/generation/models/create-account-response';
import { CreateAccountRequest, SearchAccountResponse, UpdateAccountRequest } from '../api/generation/models';

@Injectable({
  providedIn: 'root'
})
export class AppAccountService {
  private accountService = inject(AccountService);
  private router = inject(Router);
  
  accounts = signal<SearchAccountResponse[]>([]);
  selectedAccount = signal<GetAccountByIdResponse | null>(null);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  
  totalBalance = computed(() => {
    return this.accounts().reduce((sum, account) => sum + (account.balance || 0), 0);
  });
  
  constructor() {
    effect(() => {
      if (this.selectedAccount()) {
        console.log('Selected account:', this.selectedAccount());
      }
    });
  }
  
  loadAccounts() {
    this.loading.set(true);
    this.error.set(null);
    
    this.accountService.searchAccount({
      body: {
        pageNumber: 1,
        pageSize: 100
      }
    }).subscribe({
      next: (response) => {
        this.accounts.set(response.data || []);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to load accounts');
        this.loading.set(false);
      }
    });
  }
  
  loadAccount(id: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.accountService.getAccountById({ id: id }).subscribe({
      next: (account) => {
        this.selectedAccount.set(account);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to load account');
        this.loading.set(false);
      }
    });
  }
  
  createAccount(account: CreateAccountRequest) {
    this.loading.set(true);
    this.error.set(null);
    
    this.accountService.createAccount({ body: account }).subscribe({
      next: (response: CreateAccountResponse) => {
        this.loadAccounts();
        this.loading.set(false);
        this.router.navigate(['/accounts', response.id]);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to create account');
        this.loading.set(false);
      }
    });
  }
  
  updateAccount(account: UpdateAccountRequest) {
    this.loading.set(true);
    this.error.set(null);
    
    this.accountService.updateAccount({ body: account }).subscribe({
      next: () => {
        this.loadAccounts();
        if (account.id) {
          this.loadAccount(account.id);
        }
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to update account');
        this.loading.set(false);
      }
    });
  }
  
  deleteAccount(id: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.accountService.deleteAccount({ id: id }).subscribe({
      next: () => {
        this.accounts.update(accounts => 
          accounts.filter(a => a.id !== id)
        );
        if (this.selectedAccount()?.id === id) {
          this.selectedAccount.set(null);
        }
        this.loading.set(false);
        this.router.navigate(['/accounts']);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to delete account');
        this.loading.set(false);
      }
    });
  }
}