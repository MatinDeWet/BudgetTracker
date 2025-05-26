import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../../api/generation/services';

function equalValues(controlName1: string, controlName2: string) {
  return (control: AbstractControl) => {
    const val1 = control.get(controlName1)?.value;
    const val2 = control.get(controlName2)?.value;

    if (val1 === val2) {
      return null;
    }

    return { valuesNotEqual: true };
  };
}


@Component({
  selector: 'app-user-register',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './user-register.component.html',
  styleUrl: './user-register.component.css'
})
export class UserRegisterComponent {
  authService = inject(AuthService);
  router = inject(Router);

  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string>('');

  form = new FormGroup({
    email: new FormControl('',{ validators:[Validators.required, Validators.email]}),
    passwords: new FormGroup({
      password: new FormControl('',{ validators:[Validators.required]}),
      confirmPassword: new FormControl('',{ validators:[Validators.required]})
    },
    {
      validators: [equalValues('password', 'confirmPassword')],
    }),
  });
  
  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    const email = this.form.controls.email.value || '';
    const password = this.form.controls.passwords.controls.password.value || '';
    const confirmPassword = this.form.controls.passwords.controls.confirmPassword.value || '';

    this.authService.authRegister({
      body: {
        email: email,
        password: password,
        confirmPassword: confirmPassword
      }
    }).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.form.reset();
        this.router.navigate(['/auth','login']);
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(error.error.message || 'Registration failed');
      }
    });
  }
}
