import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { UserListComponent } from './user-list.component';
import { TaskListComponent } from './task-list.component';
import { TaskFilterComponent } from './task-filter.component';
import { CreateTaskDialogComponent } from './create-task-dialog.component';
import { CreateUserDialogComponent } from './create-user-dialog.component';
import { ErrorDialogComponent } from './error-dialog.component';
import { UserService } from '../services/user.service';
import { TaskService } from '../services/task.service';
import { ErrorService } from '../services/error.service';
import { User } from '../models/user.model';
import { Task } from '../models/task.model';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatListModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    UserListComponent,
    TaskListComponent,
    TaskFilterComponent
  ],
  template: `
    <div class="dashboard">
      <div class="left-panel">
        <h3>Usuarios</h3>
        <button mat-raised-button color="primary" (click)="openCreateUserDialog()">Crear Usuario</button>
        <app-user-list [users]="(users$ | async) || []" (userSelected)="onUserSelected($event)"></app-user-list>
      </div>
      <div class="right-panel">
        <h3>Tareas de {{ selectedUser?.name || 'Selecciona un usuario' }}</h3>
        <button mat-raised-button color="primary" (click)="openCreateTaskDialog()" [disabled]="!selectedUser">Crear Tarea</button>
        <app-task-filter (filterChanged)="onFilterChanged($event)"></app-task-filter>
        <app-task-list
          [tasks]="(filteredTasks$ | async) || []"
          [selectedUser]="selectedUser"
          (statusChanged)="onStatusChanged($event)"
        ></app-task-list>
      </div>
    </div>
  `,
  styles: [`
    .dashboard {
      display: flex;
      height: 100vh;
    }
    .left-panel {
      width: 30%;
      padding: 20px;
      border-right: 1px solid #ccc;
    }
    .right-panel {
      width: 70%;
      padding: 20px;
    }
    .right-panel button {
      margin-bottom: 8px;
    }
    h3 {
      margin-top: 0;
    }
  `]
})
export class DashboardComponent implements OnInit {
  users$ = new BehaviorSubject<User[]>([]);
  selectedUser: User | null = null;
  selectedFilter = '';
  filteredTasks$ = new BehaviorSubject<Task[]>([]);

  constructor(private userService: UserService, private taskService: TaskService, private dialog: MatDialog, private errorService: ErrorService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users$.next(users);
        if (users.length > 0 && !this.selectedUser) {
          this.onUserSelected(users[0]); // Seleccionar el primero por defecto solo si no hay selección
        } else if (this.selectedUser) {
          // Actualizar selectedUser con el objeto actualizado de la nueva lista
          const updatedUser = users.find(u => u.id === this.selectedUser!.id);
          if (updatedUser) {
            this.selectedUser = updatedUser;
            this.applyFilter();
          }
        }
      },
      error: (err: any) => {
        console.error('Error loading users', err);
        const detail = err.error?.detail || 'Error desconocido';
        const status = err.error?.status || err.status;
        const exceptionType = err.error?.exceptionType || '';
        const message = `Error: ${detail} (Status: ${status}${exceptionType ? ', Type: ' + exceptionType : ''})`;
        this.errorService.showError(message);
      }
    });
  }

  onUserSelected(user: User): void {
    this.selectedUser = user;
    this.applyFilter();
  }

  onFilterChanged(filter: string): void {
    this.selectedFilter = filter;
    this.applyFilter();
  }

  applyFilter(): void {
    if (this.selectedUser) {
      let tasks = this.selectedUser.tasks;
      if (this.selectedFilter) {
        tasks = tasks.filter(task => task.status === this.selectedFilter);
      }
      this.filteredTasks$.next(tasks);
    } else {
      this.filteredTasks$.next([]);
    }
  }

  onStatusChanged(task: any): void {
    // Aquí manejar el cambio de estado, pero como es en task-list, quizás emitir desde ahí
    // Por ahora, recargar usuarios para actualizar
    this.loadUsers();
  }

  openCreateUserDialog(): void {
    const dialogRef = this.dialog.open(CreateUserDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUsers();
      }
    });
  }

  openCreateTaskDialog(): void {
    if (!this.selectedUser) return;

    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '500px',
      data: { userId: this.selectedUser.id }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUsers(); // Recargar para actualizar tareas
      }
    });
  }
}