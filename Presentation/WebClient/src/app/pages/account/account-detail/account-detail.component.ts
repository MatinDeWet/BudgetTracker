import { Component, effect, inject, input, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AppAccountService } from '../../../services/AppAccountService';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UpdateAccountRequest } from '../../../api/generation/models';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { TransactionListComponent } from "../../transaction/transaction-list/transaction-list.component";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NewTransactionModalComponent } from '../../../components/transaction/new-transaction-modal/new-transaction-modal.component';

@Component({
  selector: 'app-account-detail',
  imports: [RouterLink, ReactiveFormsModule, CurrencyPipe, DatePipe, TransactionListComponent],
  templateUrl: './account-detail.component.html',
  styleUrl: './account-detail.component.css'
})
export class AccountDetailComponent implements OnInit {
  id = input.required<string>();
  
  private router = inject(Router);
  accountService = inject(AppAccountService);
  private modalService = inject(NgbModal);
  
  isEditing = signal<boolean>(false);
  showDeleteConfirmation = signal<boolean>(false);
  
  form = new FormGroup({
    accountName: new FormControl('', { validators: [Validators.required] }),
    balance: new FormControl<number>(0, { validators: [Validators.required] })
  });

  ngOnInit(): void {
    this.loadAccountDetails(this.id());
  }
  
  loadAccountDetails(accountId: string): void {
    this.accountService.loadAccount(accountId);
  }
  
  toggleEditMode(): void {
    if (this.isEditing()) {
      this.isEditing.set(false);
    } else {
      const account = this.accountService.selectedAccount();
      if (account) {
        this.form.setValue({
          accountName: account.name || '',
          balance: account.balance || 0
        });
        this.isEditing.set(true);
      }
    }
  }
  
  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }
    
    const account = this.accountService.selectedAccount();
    if (!account) return;
    
    const updatedAccount : UpdateAccountRequest = {
      id: account.id || '',
      name: this.form.controls.accountName.value || '',
    };
    
    this.accountService.updateAccount(updatedAccount);
    this.isEditing.set(false);
  }
  
  confirmDelete(): void {
    this.showDeleteConfirmation.set(true);
  }
  
  cancelDelete(): void {
    this.showDeleteConfirmation.set(false);
  }
  
  deleteAccount(): void {
    const account = this.accountService.selectedAccount();
    if (account && account.id) {
      this.accountService.deleteAccount(account.id);
      this.router.navigate(['/accounts']);
    }
  }

  openNewTransactionModal(): void {
    const account = this.accountService.selectedAccount();
    if (!account || !account.id) return;
    
    const modalRef = this.modalService.open(NewTransactionModalComponent);
    modalRef.componentInstance.accountId = account.id;
    modalRef.componentInstance.accountName = account.name;

    const accountId = account.id;
    
    modalRef.closed.subscribe(() => {
      this.loadAccountDetails(accountId);
      
      const transactionListComponent = document.querySelector('app-transaction-list');
      if (transactionListComponent) {
        const transactionList = document.querySelector('app-transaction-list') as any;
        if (transactionList && typeof transactionList.refreshTransactions === 'function') {
          transactionList.refreshTransactions();
        } else {
          transactionListComponent.dispatchEvent(new CustomEvent('refresh-transactions'));
        }
      }
    });
  }
}
