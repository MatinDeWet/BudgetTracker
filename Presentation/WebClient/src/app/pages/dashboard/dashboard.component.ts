import { Component, inject, OnInit } from '@angular/core';
import { AppAccountService } from '../../services/AppAccountService';
import { CurrencyPipe } from '@angular/common';
import { AccountSectionComponent } from '../../components/dashboard/account-section/account-section.component';

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, AccountSectionComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  accountService = inject(AppAccountService);

    ngOnInit(): void {
    this.accountService.loadAccounts();
  }
}
