import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTaskRequest, UpdateTaskStatusRequest } from '../models/task.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = environment.apiUrl + '/Task';

  constructor(private http: HttpClient) {}

  getTasks(status?: string): Observable<Task[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<Task[]>(this.apiUrl, { params });
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  createTask(task: CreateTaskRequest): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  updateTaskStatus(id: number, status: UpdateTaskStatusRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, JSON.stringify(status), { headers: { 'Content-Type': 'application/json' } });
  }
}