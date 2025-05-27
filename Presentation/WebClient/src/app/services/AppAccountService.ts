import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { AccountService } from "../api/generation/services";
import { Observable, catchError, map, of, tap } from "rxjs";
import { SearchAccountResponse } from "../api/generation/models";
import { PageableResponseOfSearchAccountResponse } from "../api/generation/models";
import { CreateAccountRequest } from "../api/generation/models";

@Injectable({ providedIn: 'root' })
export class AppAccountService {
  private accountService = inject(AccountService);
  private router = inject(Router);

  private accountsSignal = signal<SearchAccountResponse[]>([]);
  private loadingSignal = signal<boolean>(false);
  private errorSignal = signal<string | null>(null);

  public accounts = computed(() => this.accountsSignal());
  public loading = computed(() => this.loadingSignal());
  public error = computed(() => this.errorSignal());

  constructor() {}

  public clearError(): void {
    this.errorSignal.set(null);
  }

  public searchAccounts(pageNumber: number = 1, pageSize: number = 10): Observable<PageableResponseOfSearchAccountResponse> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.accountService.searchAccount({
      body: {
        pageNumber: pageNumber,
        pageSize: pageSize
      }
    }).pipe(
      tap(response => {
        this.accountsSignal.set(response.data || []);
        this.loadingSignal.set(false);
      }),
      catchError(error => {
        this.errorSignal.set('Failed to load accounts');
        this.loadingSignal.set(false);
        return of({ data: [], totalRecords: 0 } as PageableResponseOfSearchAccountResponse);
      })
    );
  }

  public createAccount(name: string): Observable<string | null> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.accountService.createAccount({
      body: { name }
    }).pipe(
      map(response => {
        this.loadingSignal.set(false);
        // After creating, refresh the accounts list
        this.searchAccounts().subscribe();
        return response.id || null;
      }),
      catchError(error => {
        this.errorSignal.set('Failed to create account');
        this.loadingSignal.set(false);
        return of(null);
      })
    );
  }

  public getAccountById(id: string): Observable<any> {
    return this.accountService.getAccountById({ id });
  }

  public updateAccount(id: string, name: string): Observable<void> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.accountService.updateAccount({
      body: { id, name }
    }).pipe(
      tap(() => {
        this.loadingSignal.set(false);
        // After updating, refresh the accounts list
        this.searchAccounts().subscribe();
      }),
      catchError(error => {
        this.errorSignal.set('Failed to update account');
        this.loadingSignal.set(false);
        return of(undefined);
      })
    );
  }

  public deleteAccount(id: string): Observable<void> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    return this.accountService.deleteAccount({ id }).pipe(
      tap(() => {
        this.loadingSignal.set(false);
        // After deletion, refresh the accounts list
        this.searchAccounts().subscribe();
      }),
      catchError(error => {
        this.errorSignal.set('Failed to delete account');
        this.loadingSignal.set(false);
        return of(undefined);
      })
    );
  }
}