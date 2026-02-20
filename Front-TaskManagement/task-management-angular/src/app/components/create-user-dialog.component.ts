import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { ErrorService } from '../services/error.service';

@Component({
  selector: 'app-create-user-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  template: `
    <h2 mat-dialog-title>Crear Usuario</h2>
    <mat-dialog-content>
      <form [formGroup]="userForm">
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Nombre</mat-label>
          <input matInput formControlName="name" required>
          <mat-error *ngIf="userForm.get('name')?.invalid && userForm.get('name')?.touched">
            Nombre es requerido
          </mat-error>
        </mat-form-field>
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Email</mat-label>
          <input matInput formControlName="email" type="email" required>
          <mat-error *ngIf="userForm.get('email')?.invalid && userForm.get('email')?.touched">
            Email válido es requerido
          </mat-error>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancelar</button>
      <button mat-raised-button color="primary" (click)="onSubmit()" [disabled]="userForm.invalid">Crear</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .full-width {
      width: 100%;
    }
  `]
})
export class CreateUserDialogComponent {
  userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private errorService: ErrorService,
    private dialogRef: MatDialogRef<CreateUserDialogComponent>
  ) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      this.userService.createUser(this.userForm.value).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (err: any) => {
          console.error('Error creating user', err);
          const detail = err.error?.detail || 'Error desconocido';
          const status = err.error?.status || err.status;
          const exceptionType = err.error?.exceptionType || '';
          const message = `Error: ${detail} (Status: ${status}${exceptionType ? ', Type: ' + exceptionType : ''})`;
          this.errorService.showError(message);
        }
      });
    }
  }
}