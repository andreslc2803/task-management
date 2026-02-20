import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../services/task.service';
import { ErrorService } from '../services/error.service';

@Component({
  selector: 'app-create-task-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    ReactiveFormsModule
  ],
  template: `
    <h2 mat-dialog-title>Crear Tarea</h2>
    <mat-dialog-content>
      <form [formGroup]="taskForm">
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Título</mat-label>
          <input matInput formControlName="title" required>
          <mat-error *ngIf="taskForm.get('title')?.invalid && taskForm.get('title')?.touched">
            Título es requerido
          </mat-error>
        </mat-form-field>
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Descripción</mat-label>
          <textarea matInput formControlName="description" rows="3"></textarea>
        </mat-form-field>
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Estado</mat-label>
          <mat-select formControlName="status" required>
            <mat-option value="Pending">Pendiente</mat-option>
            <mat-option value="InProgress">En Progreso</mat-option>
            <mat-option value="Completed">Completada</mat-option>
          </mat-select>
          <mat-error *ngIf="taskForm.get('status')?.invalid && taskForm.get('status')?.touched">
            Estado es requerido
          </mat-error>
        </mat-form-field>
        <mat-form-field appearance="fill" class="full-width">
          <mat-label>Nivel de Prioridad</mat-label>
          <mat-select formControlName="priority">
            <mat-option value="low">Low</mat-option>
            <mat-option value="medium">Medium</mat-option>
            <mat-option value="high">High</mat-option>
          </mat-select>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancelar</button>
      <button mat-raised-button color="primary" (click)="onSubmit()" [disabled]="taskForm.invalid">Crear</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .full-width {
      width: 100%;
    }
  `]
})
export class CreateTaskDialogComponent {
  taskForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private errorService: ErrorService,
    private dialogRef: MatDialogRef<CreateTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: number }
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      status: ['Pending', Validators.required],
      priority: ['']
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskPriority = formValue.priority ? JSON.stringify({ priority: formValue.priority }) : undefined;

      const createRequest = {
        title: formValue.title,
        description: formValue.description || undefined,
        status: formValue.status,
        userId: this.data.userId,
        taskPriority: taskPriority
      };

      this.taskService.createTask(createRequest).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (err: any) => {
          console.error('Error creating task', err);
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