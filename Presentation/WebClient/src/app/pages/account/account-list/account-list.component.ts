import { Component, inject, OnInit, signal } from '@angular/core';
import { AppAccountService } from '../../../services/AppAccountService';
import { NewAccountModalComponent } from '../../../components/account/new-account-modal/new-account-modal.component';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-account-list',
  imports: [RouterLink, CurrencyPipe, NewAccountModalComponent],
  templateUrl: './account-list.component.html',
  styleUrl: './account-list.component.css'
})
export class AccountListComponent implements OnInit{
  accountService = inject(AppAccountService);
  isNewAccountModalVisible = signal(false);
  
  ngOnInit(): void {
    this.loadAccounts();
  }
  
  loadAccounts(): void {
    this.accountService.loadAccounts();
  }
  
  openNewAccountModal(): void {
    this.isNewAccountModalVisible.set(true);
  }
  
  closeNewAccountModal(): void {
    this.isNewAccountModalVisible.set(false);
  } 
}
