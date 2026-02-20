import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { User } from '../models/user.model';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatListModule, MatPaginatorModule],
  template: `
    <mat-card *ngFor="let user of paginatedUsers" [class.selected]="user === selectedUser" (click)="selectUser(user)">
      <mat-card-header>
        <mat-card-title>{{ user.name }}</mat-card-title>
        <mat-card-subtitle>{{ user.email }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <p>Creado: {{ user.createAt | date }}</p>
        <p>Tareas asignadas: {{ user.tasks.length }}</p>
      </mat-card-content>
    </mat-card>
    <mat-paginator
      [length]="users.length"
      [pageSize]="pageSize"
      [pageSizeOptions]="[5, 10, 20]"
      (page)="onPageChange($event)">
    </mat-paginator>
  `,
  styles: [`
    mat-card {
      margin: 10px 0;
      cursor: pointer;
    }
    mat-card.selected {
      background-color: #e3f2fd;
    }
  `]
})
export class UserListComponent {
  @Input() users: User[] = [];
  @Output() userSelected = new EventEmitter<User>();
  selectedUser: User | null = null;
  pageIndex = 0;
  pageSize = 5;

  get paginatedUsers(): User[] {
    const start = this.pageIndex * this.pageSize;
    return this.users.slice(start, start + this.pageSize);
  }

  selectUser(user: User): void {
    this.selectedUser = user;
    this.userSelected.emit(user);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
  }
}