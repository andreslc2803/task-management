export interface Task {
  id: number;
  title: string;
  status: string;
  taskPriority: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  status: string;
  userId: number;
  taskPriority?: string;
}

export type UpdateTaskStatusRequest = string;