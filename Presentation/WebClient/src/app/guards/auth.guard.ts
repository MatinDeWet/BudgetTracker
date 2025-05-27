import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AppAuthService } from '../services/AppAuthService';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AppAuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  router.navigate(['/auth','login']);
  
  return false;
};