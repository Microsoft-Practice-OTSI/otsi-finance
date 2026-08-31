import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/api.service';
import { CreateEmployee, Department, Employee } from '../../core/models';
import { PageHeaderComponent } from '../../shared/page-header.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  template: `
    <app-page-header title="Employees" subtitle="People, roles, and compensation"></app-page-header>
    <div class="card">
      <div class="form-section">
        <h3>Identity</h3>
        <div class="form-row">
          <input [(ngModel)]="newEmp.employeeId" placeholder="Employee id" />
          <input [(ngModel)]="newEmp.firstName" placeholder="First name" />
          <input [(ngModel)]="newEmp.lastName" placeholder="Last name" />
          <input [(ngModel)]="newEmp.email" placeholder="Email" />
        </div>
      </div>

      <div class="form-section">
        <h3>Job</h3>
        <div class="form-row">
          <input [(ngModel)]="newEmp.jobTitle" placeholder="Job title" />
          <select [(ngModel)]="newEmp.departmentId">
            <option [ngValue]="0" disabled>Department</option>
            <option *ngFor="let d of departments" [ngValue]="d.id">{{ d.name }}</option>
          </select>
        </div>
      </div>

      <div class="form-section">
        <h3>Compensation</h3>
        <div class="form-row">
          <select [(ngModel)]="newEmp.payType">
            <option [ngValue]="0">Monthly</option>
            <option [ngValue]="1">Hourly</option>
          </select>
          <input *ngIf="newEmp.payType === 0" [(ngModel)]="newEmp.annualSalary" type="number" placeholder="Annual salary" />
          <input *ngIf="newEmp.payType === 1" [(ngModel)]="newEmp.payRate" type="number" placeholder="Pay rate" />
        </div>
      </div>

      <div class="form-section">
        <h3>Status</h3>
        <div class="form-row">
          <select [(ngModel)]="newEmp.status">
            <option [ngValue]="0">Active</option>
            <option [ngValue]="1">Inactive</option>
            <option [ngValue]="2">Terminated</option>
          </select>
          <input [(ngModel)]="newEmp.hireDate" type="date" placeholder="Hire date" />
          <button (click)="add()">Add</button>
        </div>
      </div>

      <p class="error" *ngIf="error">{{ error }}</p>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>#</th><th>Name</th><th>Title</th><th>Department</th><th>Pay</th></tr></thead>
        <tbody>
          <tr *ngFor="let e of employees">
            <td>{{ e.employeeId }}</td>
            <td>{{ e.fullName }}</td>
            <td>{{ e.jobTitle }}</td>
            <td>{{ e.departmentName }}</td>
            <td>{{ e.payType === 0 ? ('$' + e.annualSalary + '/mo') : ('$' + e.payRate + '/hr') }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `
})
export class EmployeesComponent implements OnInit {
  employees: Employee[] = [];
  departments: Department[] = [];
  newEmp: CreateEmployee = {
    employeeId: '', firstName: '', lastName: '', email: '',
    jobTitle: '', departmentId: 0, payType: 0, annualSalary: 0, payRate: 0, status: 0, hireDate: ''
  };
  error = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.get<Department[]>('departments').subscribe((d) => (this.departments = d));
    this.load();
  }

  load(): void {
    this.api.get<Employee[]>('employees').subscribe((e) => (this.employees = e));
  }

  add(): void {
    this.error = '';
    this.api.post<Employee>('employees', this.newEmp).subscribe({
      next: () => {
        this.newEmp = {
          employeeId: '', firstName: '', lastName: '', email: '',
          jobTitle: '', departmentId: 0, payType: 0, annualSalary: 0, payRate: 0, status: 0, hireDate: ''
        };
        this.load();
      },
      error: (e) => (this.error = e.error ?? 'Failed to add employee.')
    });
  }
}
