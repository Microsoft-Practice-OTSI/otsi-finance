import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/api.service';
import { CreateDepartment, Department } from '../../core/models';
import { PageHeaderComponent } from '../../shared/page-header.component';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  template: `
    <app-page-header title="Departments" subtitle="Create and manage organizational units"></app-page-header>
    <div class="card">
      <div class="form-row">
        <input [(ngModel)]="newDept.name" placeholder="Name" />
        <input [(ngModel)]="newDept.code" placeholder="Code" />
        <input [(ngModel)]="newDept.description" placeholder="Description" />
        <button (click)="add()">Add</button>
      </div>
      <p class="error" *ngIf="error">{{ error }}</p>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Code</th><th>Name</th><th>Description</th></tr></thead>
        <tbody>
          <tr *ngFor="let d of departments">
            <td>{{ d.code }}</td><td>{{ d.name }}</td><td>{{ d.description }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `
})
export class DepartmentsComponent implements OnInit {
  departments: Department[] = [];
  newDept: CreateDepartment = { name: '', code: '', description: '' };
  error = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.api.get<Department[]>('departments').subscribe((d) => (this.departments = d));
  }

  add(): void {
    this.error = '';
    this.api.post<Department>('departments', this.newDept).subscribe({
      next: () => {
        this.newDept = { name: '', code: '', description: '' };
        this.load();
      },
      error: (e) => (this.error = e.error ?? 'Failed to add department.')
    });
  }
}
