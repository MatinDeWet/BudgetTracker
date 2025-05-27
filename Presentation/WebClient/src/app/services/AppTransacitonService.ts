import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { TransactionService } from "../api/generation/services";
import { Observable, catchError, map, of, tap } from "rxjs";
import { PageableResponseOfSeachTransactionResponse } from "../api/generation/models";
import { GetTransactionByIdResponse } from "../api/generation/models";

@Injectable({ providedIn: 'root' })
export class AppTransactionService {
  private transactionService = inject(TransactionService);
  private router = inject(Router);

  private transactionsSignal = signal<any[]>([]);
  private loadingSignal = signal<boolean>(false);
  private errorSignal = signal<string | null>(null);

  public transactions = computed(() => this.transactionsSignal());
  public loading = computed(() => this.loadingSignal());
  public error = computed(() => this.errorSignal());

  constructor() {}

  public clearError(): void {
    this.errorSignal.set(null);
  }

  public searchTransactions(
    accountId: string, 
    pageNumber: number = 1, 
    pageSize: number = 10
  ): Observable<PageableResponseOfSeachTransactionResponse> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.transactionService.searchTransaction({
      body: {
        accountId: accountId,
        pageNumber: pageNumber,
        pageSize: pageSize
      }
    }).pipe(
      tap(response => {
        this.transactionsSignal.set(response.data || []);
        this.loadingSignal.set(false);
      }),
      catchError(error => {
        this.errorSignal.set('Failed to load transactions');
        this.loadingSignal.set(false);
        return of({ data: [], totalRecords: 0 } as PageableResponseOfSeachTransactionResponse);
      })
    );
  }

  public getTransactionById(id: string): Observable<GetTransactionByIdResponse> {
    return this.transactionService.getTransactionById({ id });
  }

  public createTransaction(
    accountId: string, 
    amount: number, 
    description: string, 
    date: string
  ): Observable<string | null> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.transactionService.createTransaction({
      body: { 
        accountId, 
        amount, 
        description, 
        date 
      }
    }).pipe(
      map(response => {
        this.loadingSignal.set(false);
        return response.id || null;
      }),
      catchError(error => {
        this.errorSignal.set('Failed to create transaction');
        this.loadingSignal.set(false);
        return of(null);
      })
    );
  }

  public updateTransaction(
    id: string, 
    amount: number, 
    description: string, 
    date: string
  ): Observable<void> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.transactionService.updateTransaction({
      body: { 
        id, 
        amount, 
        description, 
        date 
      }
    }).pipe(
      tap(() => {
        this.loadingSignal.set(false);
      }),
      catchError(error => {
        this.errorSignal.set('Failed to update transaction');
        this.loadingSignal.set(false);
        return of(undefined);
      })
    );
  }

  public deleteTransaction(id: string): Observable<void> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.transactionService.deleteTransaction({ id }).pipe(
      tap(() => {
        this.loadingSignal.set(false);
      }),
      catchError(error => {
        this.errorSignal.set('Failed to delete transaction');
        this.loadingSignal.set(false);
        return of(undefined);
      })
    );
  }

  public tagTransaction(transactionId: string, tagId: string): Observable<void> {
    return this.transactionService.tagTransaction({
      body: { 
        transactionId, 
        tagId 
      }
    });
  }

  public untagTransaction(transactionId: string, tagId: string): Observable<void> {
    return this.transactionService.untagTransaction({
      body: { 
        transactionId, 
        tagId 
      }
    });
  }
}