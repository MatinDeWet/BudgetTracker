import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppAccountService } from '../../../services/AppAccountService';
import { CreateAccountRequest } from '../../../api/generation/models';

@Component({
  selector: 'app-new-account-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './new-account-modal.component.html',
  styleUrl: './new-account-modal.component.css'
})
export class NewAccountModalComponent {
  isVisible = input<boolean>(false);
  closeModal = output<void>();
  
  private fb = inject(FormBuilder);
  accountService = inject(AppAccountService);
  
  accountForm: FormGroup = this.fb.group({
    name: ['', [Validators.required]]
  });
  
  ngOnInit(): void {
    this.resetForm();
  }
  
  resetForm(): void {
    this.accountForm.reset({ name: '' });
  }
  
  close(): void {
    this.closeModal.emit();
  }
  
  createAccount(): void {
    if (this.accountForm.valid) {
      const newAccount : CreateAccountRequest = {
        name: this.accountForm.value.name
      };
      
      this.accountService.createAccount(newAccount);
      this.close();
    }
  }
}
