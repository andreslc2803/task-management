import { Task } from './task.model';

export interface User {
  id: number;
  name: string;
  email: string;
  createAt: string;
  tasks: Task[];
}

export interface CreateUserRequest {
  name: string;
  email: string;
}