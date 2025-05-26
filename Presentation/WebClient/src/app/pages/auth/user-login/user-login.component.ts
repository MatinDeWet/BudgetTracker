import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../api/generation/services';

@Component({
  selector: 'app-user-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  authService = inject(AuthService);
  router = inject(Router);

  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string>('');

  form = new FormGroup({
    email: new FormControl('',{ validators:[Validators.required, Validators.email]}),
    password: new FormControl('',{ validators:[Validators.required]}),
  });

    onSubmit() {
      if (this.form.invalid) {
        return;
      }

          this.isSubmitting.set(true);
    this.errorMessage.set('');

    const email = this.form.controls.email.value || '';
    const password = this.form.controls.password.value || '';

    this.authService.authLogin({
      body: {
        email: email,
        password: password
      }
    }).subscribe({
      next: (response) => {
        // Save token to localStorage
        localStorage.setItem('auth_acces_token', response.accessToken || '');
        localStorage.setItem('auth_refresh_token', response.refreshToken || '');
        localStorage.setItem('auth_user_id', response.userId || '');
        
        this.isSubmitting.set(false);
        // Navigate to home or dashboard page after successful login
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(error.error?.message || 'Login failed. Please check your credentials.');
      }
    });
    }
}
