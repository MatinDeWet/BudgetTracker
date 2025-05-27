import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AppAuthService } from '../../../services/AppAuthService';

@Component({
  selector: 'app-user-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  authService = inject(AppAuthService);
  router = inject(Router);

  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string>('');
  showPassword = false;

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

    this.authService.login(email, password).subscribe({
        // You can leave this empty since navigation is handled in the service
        next: () => {
          this.isSubmitting.set(false);
        },
        error: (error) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(error.error?.message || 'Login failed');
        }
      });
    }
}
