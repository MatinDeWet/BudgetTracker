import { HttpErrorResponse } from "@angular/common/http";
import { CreateTransactionRequest, CreateTransactionResponse, GetTransactionByIdResponse, PageableResponseOfSeachTransactionResponse, SeachTransactionRequest, SeachTransactionResponse, UpdateTransactionRequest } from "../api/generation/models";
import { computed, effect, inject, Injectable, signal } from "@angular/core";
import { TransactionService } from "../api/generation/services";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AppTransactionService {
  private transactionService = inject(TransactionService);
  private router = inject(Router);
  
  transactions = signal<SeachTransactionResponse[]>([]);
  selectedTransaction = signal<GetTransactionByIdResponse | null>(null);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  
  currentPage = signal<number>(1);
  pageSize = signal<number>(10);
  totalItems = signal<number>(0);
  totalPages = signal<number>(0);
  
  totalAmount = computed(() => {
    return this.transactions().reduce((sum, tx) => sum + (tx.amount || 0), 0);
  });
  
  constructor() {
    effect(() => {
      if (this.selectedTransaction()) {
        console.log('Selected transaction:', this.selectedTransaction());
      }
    });
  }
  
  searchTransactions(options: SeachTransactionRequest = {}) {
    this.loading.set(true);
    this.error.set(null);
    
    const page = options.pageNumber || this.currentPage();
    const size = options.pageSize || this.pageSize();
    
    this.transactionService.searchTransaction({
      body: {
        accountId: options.accountId,
        dateRange: options.dateRange,
        orderBy: options.orderBy,
        orderDirection: options.orderDirection,
        transactionDirection: options.transactionDirection,
        pageNumber: page,
        pageSize: size
      }
    }).subscribe({
      next: (response: PageableResponseOfSeachTransactionResponse) => {
        this.transactions.set(response.data || []);
        this.totalItems.set(response.totalRecords || 0);
        this.totalPages.set(response.pageCount || 0);
        this.currentPage.set(page);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to load transactions');
        this.loading.set(false);
      }
    });
  }
  
  loadTransaction(id: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.transactionService.getTransactionById({ id: id }).subscribe({
      next: (transaction) => {
        this.selectedTransaction.set(transaction);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to load transaction');
        this.loading.set(false);
      }
    });
  }
  
  createTransaction(transaction: CreateTransactionRequest) {
  this.loading.set(true);
  this.error.set(null);
  
  this.transactionService.createTransaction({ body: transaction }).subscribe({
    next: (response: CreateTransactionResponse) => {
      this.searchTransactions({
        accountId: transaction.accountId
      });
      this.loading.set(false);
      this.router.navigate(['/accounts', transaction.accountId]);
    },
    error: (error: HttpErrorResponse) => {
      this.error.set(error.message || 'Failed to create transaction');
      this.loading.set(false);
    }
  });
}
  
  updateTransaction(transaction: UpdateTransactionRequest) {
    this.loading.set(true);
    this.error.set(null);
    
    this.transactionService.updateTransaction({ body: transaction }).subscribe({
      next: () => {
        this.searchTransactions();
        if (transaction.id) {
          this.loadTransaction(transaction.id);
        }
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to update transaction');
        this.loading.set(false);
      }
    });
  }
  
  deleteTransaction(id: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.transactionService.deleteTransaction({ id: id }).subscribe({
      next: () => {
        this.transactions.update(transactions => 
          transactions.filter(t => t.id !== id)
        );
        if (this.selectedTransaction()?.id === id) {
          this.selectedTransaction.set(null);
        }
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to delete transaction');
        this.loading.set(false);
      }
    });
  }
  
  addTag(transactionId: string, tagId: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.transactionService.tagTransaction({
      body: {
        transactionId: transactionId,
        tagId: tagId
      }
    }).subscribe({
      next: () => {
        if (this.selectedTransaction()?.id === transactionId) {
          this.loadTransaction(transactionId);
        }
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to add tag');
        this.loading.set(false);
      }
    });
  }
  
  removeTag(transactionId: string, tagId: string) {
    this.loading.set(true);
    this.error.set(null);
    
    this.transactionService.untagTransaction({
      body: {
        transactionId: transactionId,
        tagId: tagId
      }
    }).subscribe({
      next: () => {
        if (this.selectedTransaction()?.id === transactionId) {
          this.loadTransaction(transactionId);
        }
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(error.message || 'Failed to remove tag');
        this.loading.set(false);
      }
    });
  }
  
  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.searchTransactions({ pageNumber: this.currentPage() + 1 });
    }
  }
  
  previousPage(): void {
    if (this.currentPage() > 1) {
      this.searchTransactions({ pageNumber: this.currentPage() - 1 });
    }
  }
  
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.searchTransactions({ pageNumber: page });
    }
  }
  
  clearFilters(): void {
    this.searchTransactions({ pageNumber: 1 });
  }
}