import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppTransactionService } from '../../../services/AppTransactionService';

@Component({
  selector: 'app-new-transaction-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './new-transaction-modal.component.html',
  styleUrl: './new-transaction-modal.component.css'
})
export class NewTransactionModalComponent implements OnInit {
  @Input() accountId!: string;
  @Input() accountName: string = '';

  transactionForm!: FormGroup;
  today = new Date().toISOString().split('T')[0];
  
  private fb = inject(FormBuilder);
  public activeModal = inject(NgbActiveModal);
  transactionService = inject(AppTransactionService);

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.transactionForm = this.fb.group({
      accountId: [this.accountId, Validators.required],
      description: ['', [Validators.required, Validators.maxLength(512)]],
      amount: [null, [Validators.required]],
      isExpense: [true],
      date: [this.today, Validators.required]
    });
  }

  get isFormValid(): boolean {
    return this.transactionForm.valid;
  }

  onSubmit(): void {
    if (this.transactionForm.invalid) {
      Object.keys(this.transactionForm.controls).forEach(key => {
        const control = this.transactionForm.get(key);
        control?.markAsTouched();
      });
      return;
    }

    const formData = this.transactionForm.value;
    
    let amount = Math.abs(parseFloat(formData.amount));
    if (formData.isExpense) {
      amount = -amount;
    }

    const transaction = {
      accountId: this.accountId,
      description: formData.description,
      amount: amount,
      date: new Date(formData.date).toISOString()
    };

    this.transactionService.createTransaction(transaction);
    this.activeModal.close();
  }
}