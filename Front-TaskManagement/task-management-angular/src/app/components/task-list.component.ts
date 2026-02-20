import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { TaskService } from '../services/task.service';
import { ErrorService } from '../services/error.service';
import { Task } from '../models/task.model';
import { User } from '../models/user.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  template: `
    <mat-card *ngFor="let task of tasks">
      <mat-card-header>
        <mat-card-title>{{ task.title }}</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <p>Estado: {{ task.status }}</p>
        <p>Prioridad: {{ task.taskPriority }}</p>
      </mat-card-content>
      <mat-card-actions>
        <button mat-button (click)="changeStatus(task, 'Pending')" *ngIf="task.status !== 'Pending'">Pendiente</button>
        <button mat-button (click)="changeStatus(task, 'InProgress')" *ngIf="task.status !== 'InProgress'">En Progreso</button>
        <button mat-button (click)="changeStatus(task, 'Done')" *ngIf="task.status !== 'Done'">Completada</button>
      </mat-card-actions>
    </mat-card>
  `,
  styles: [`
    mat-card { margin: 10px; }
  `]
})
export class TaskListComponent {
  @Input() tasks: Task[] = [];
  @Input() selectedUser: User | null = null;
  @Output() statusChanged = new EventEmitter<Task>();

  constructor(private taskService: TaskService, private errorService: ErrorService) {}

  changeStatus(task: Task, status: string): void {
    this.taskService.updateTaskStatus(task.id, status).subscribe({
      next: () => this.statusChanged.emit(task),
      error: (err: any) => {
        console.error('Error updating status', err);
        const detail = err.error?.detail || 'Error desconocido';
        const status = err.error?.status || err.status;
        const exceptionType = err.error?.exceptionType || '';
        const message = `Error: ${detail} (Status: ${status}${exceptionType ? ', Type: ' + exceptionType : ''})`;
        this.errorService.showError(message);
      }
    });
  }
}