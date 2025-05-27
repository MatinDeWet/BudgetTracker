import { Component, inject, signal } from '@angular/core';
import { AppAccountService } from '../../../services/AppAccountService';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NewAccountModalComponent } from "../../account/new-account-modal/new-account-modal.component";

@Component({
  selector: 'app-account-section',
  imports: [RouterLink, CurrencyPipe, NewAccountModalComponent],
  templateUrl: './account-section.component.html',
  styleUrl: './account-section.component.css'
})
export class AccountSectionComponent {
  accountService = inject(AppAccountService);

  isNewAccountModalVisible = signal(false);
  
  openNewAccountModal(): void {
    this.isNewAccountModalVisible.set(true);
  }
  
  closeNewAccountModal(): void {
    this.isNewAccountModalVisible.set(false);
  }
}
