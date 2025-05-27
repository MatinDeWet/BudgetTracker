import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../api/generation/services";
import { Observable, tap } from "rxjs";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({ providedIn: 'root' })
export class AppAuthService{
  private authService = inject(AuthService);
  private router = inject(Router);

  private isAuthenticatedSignal = signal<boolean>(this.checkIsAuthenticated());
  
  public isAuthenticated = computed(() => this.isAuthenticatedSignal());

  public login(email: string, password: string): Observable<any> {
      return this.authService.authLogin({
        body: { email, password }
      }).pipe(
        tap(response => {
          localStorage.setItem('auth_acces_token', response.accessToken || '');
          localStorage.setItem('auth_refresh_token', response.refreshToken || '');
          localStorage.setItem('auth_user_id', response.userId || '');
          
          this.updateAuthState();
          
          this.router.navigate(['/dashboard']);
        })
      );
  }

  public register(email: string, password: string, confirmPassword: string): Observable<any> {
    return this.authService.authRegister({
      body: {
        email,
        password,
        confirmPassword
      }
    });
  }

  public checkIsAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }

    return !this.isTokenExpired(token);
  }

  public getToken(): string | null {
    return localStorage.getItem('auth_acces_token');
  }

  private isTokenExpired(token: string): boolean {
    const jwtHelper = new JwtHelperService;

    const IsExpired = jwtHelper.isTokenExpired(token);

    return IsExpired;
  }

  public updateAuthState(): void {
    this.isAuthenticatedSignal.set(this.checkIsAuthenticated());
  }

  public logout(): void {
    localStorage.removeItem('auth_acces_token');
    localStorage.removeItem('auth_refresh_token');
    localStorage.removeItem('auth_user_id');
    this.updateAuthState();
    this.router.navigate(['/auth/login']);
  }
}