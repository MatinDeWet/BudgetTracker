import { Component, HostListener, OnInit, TemplateRef, effect, inject, input } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { AppTransactionService } from '../../../services/AppTransactionService';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrderDirectionEnum } from '../../../api/generation/models';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, DatePipe, FormsModule, ReactiveFormsModule],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.css'
})
export class TransactionListComponent {
  transactionService = inject(AppTransactionService);
  private modalService = inject(NgbModal);

  accountId = input<string>('');

  Math = Math;

  transactionToDelete: any = null;
  
  pageSizes = [10, 20, 50, 100];

  constructor() {
    effect(() => {
      const id = this.accountId();
      console.log('TransactionList: Account ID changed to', id);
      
      this.transactionService.currentPage.set(1);
      
      this.transactionService.transactions.set([]);
      
      if (id) {
        this.loadTransactions();
      }
    });
  }

  @HostListener('refresh-transactions')
  refreshTransactions(): void {
    console.log('Refreshing transactions for account:', this.accountId());
    this.loadTransactions();
  }
  
  loadTransactions(): void {
    const id = this.accountId();
    if (!id) return;
    
    this.transactionService.searchTransactions({
      accountId: id,
      pageNumber: this.transactionService.currentPage(),
      pageSize: this.transactionService.pageSize(),
      orderBy: 'date',
      orderDirection: OrderDirectionEnum.Descending
    });
  }
  
  nextPage(): void {
    this.transactionService.nextPage();
  }
  
  previousPage(): void {
    this.transactionService.previousPage();
  }
  
  goToPage(page: number): void {
    this.transactionService.goToPage(page);
  }
  
  changePageSize(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const newSize = parseInt(selectElement.value);
    
    this.transactionService.pageSize.set(newSize);
    this.transactionService.goToPage(1);
  }

  openDeleteConfirmation(transaction: any, content: TemplateRef<any>): void {
    this.transactionToDelete = transaction;
    this.modalService.open(content, { ariaLabelledBy: 'modal-delete-title' }).result.then(
      (result) => {
        if (result === 'confirm') {
          this.deleteTransaction();
        }
      },
      () => {
      }
    );
  }

  deleteTransaction(): void {
    if (this.transactionToDelete && this.transactionToDelete.id) {
      this.transactionService.deleteTransaction(this.transactionToDelete.id);
      this.transactionToDelete = null;
    }
  }
  
  getPageNumbers(): number[] {
    const totalPages = this.transactionService.totalPages();
    const currentPage = this.transactionService.currentPage();
    
    if (totalPages <= 7) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }
    
    let pages: number[] = [1];
    
    const start = Math.max(2, currentPage - 1);
    const end = Math.min(totalPages - 1, currentPage + 1);
    
    if (start > 2) {
      pages.push(-1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    
    if (end < totalPages - 1) {
      pages.push(-2); // 
    }
    
    pages.push(totalPages);
    
    return pages;
  }
}