import { CommonModule } from '@angular/common';
import { Component, inject, Input, output, signal, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AppTransactionService } from '../../../services/AppTransacitonService';
import { AppAccountService } from '../../../services/AppAccountService';

@Component({
  selector: 'app-transaction-list',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.css'
})
export class TransactionListComponent {
  @Input() accountId!: string;
  transactionDeleted = output<void>();
  
  transactionService = inject(AppTransactionService);
  
  transactions = signal<any[]>([]);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  
  currentPage = signal<number>(1);
  pageSize = signal<number>(5);
  totalRecords = signal<number>(0);
  totalPages = signal<number>(0);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['accountId'] && this.accountId) {
      this.loadTransactions();
    }
  }
  
  loadTransactions(): void {
    if (!this.accountId) return;
    
    this.isLoading.set(true);
    this.errorMessage.set(null);
    
    this.transactionService.searchTransactions(
      this.accountId,
      this.currentPage(),
      this.pageSize()
    ).subscribe({
      next: (response) => {
        this.transactions.set(response.data || []);
        this.totalRecords.set(response.totalRecords || 0);
        this.totalPages.set(Math.ceil((response.totalRecords || 0) / this.pageSize()));
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading transactions:', err);
        this.errorMessage.set('Failed to load transactions');
        this.isLoading.set(false);
      }
    });
  }
  
  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages() || page === this.currentPage()) return;
    
    this.currentPage.set(page);
    this.loadTransactions();
  }
  
  nextPage(): void {
    this.goToPage(this.currentPage() + 1);
  }
  
  previousPage(): void {
    this.goToPage(this.currentPage() - 1);
  }

  getPageNumbers(): number[] {
    const pageCount = this.totalPages();
    return Array.from({ length: pageCount }, (_, i) => i + 1);
  } 
  
  deleteTransaction(id: string, event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    
    if (confirm('Are you sure you want to delete this transaction?')) {
      this.transactionService.deleteTransaction(id).subscribe({
        next: () => {
          this.loadTransactions();
          this.transactionDeleted.emit();
        },
        error: (err) => {
          console.error('Error deleting transaction:', err);
          this.errorMessage.set('Failed to delete transaction');
        }
      });
    }
  }
}
