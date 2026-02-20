import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-task-filter',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatSelectModule],
  template: `
    <mat-form-field>
      <mat-label>Filtrar por estado</mat-label>
      <mat-select [(ngModel)]="selectedFilter" (selectionChange)="onFilterChange()">
        <mat-option value="">Todos</mat-option>
        <mat-option value="Pending">Pendiente</mat-option>
        <mat-option value="InProgress">En Progreso</mat-option>
        <mat-option value="Done">Completada</mat-option>
      </mat-select>
    </mat-form-field>
  `,
  styles: [`
    mat-form-field { width: 100%; }
  `]
})
export class TaskFilterComponent {
  @Output() filterChanged = new EventEmitter<string>();
  selectedFilter = '';

  onFilterChange(): void {
    this.filterChanged.emit(this.selectedFilter);
  }
}