import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/api.service';
import { Payslip } from '../../core/models';
import { PageHeaderComponent } from '../../shared/page-header.component';

@Component({
  selector: 'app-payslips',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  template: `
    <app-page-header title="Payslips" subtitle="Look up generated payslips"></app-page-header>
    <div class="card">
      <div class="form-row">
        <input [(ngModel)]="employeeId" type="number" placeholder="Employee id" />
        <button (click)="search()">Search</button>
      </div>
      <p class="error" *ngIf="error">{{ error }}</p>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Employee</th><th>Regular</th><th>Overtime</th><th>Gross</th><th>Tax</th><th>Net</th><th>Pay date</th></tr></thead>
        <tbody>
          <tr *ngFor="let p of payslips">
            <td>{{ p.employeeName }} ({{ p.employeeId }})</td>
            <td>{{ p.regularPay | number:'1.2-2' }}</td>
            <td>{{ p.overtimePay | number:'1.2-2' }}</td>
            <td>{{ p.grossPay | number:'1.2-2' }}</td>
            <td>{{ p.taxDeduction | number:'1.2-2' }}</td>
            <td>{{ p.netPay | number:'1.2-2' }}</td>
            <td>{{ p.payDate | slice:0:10 }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `
})
export class PayslipsComponent {
  payslips: Payslip[] = [];
  employeeId = '';
  error = '';

  constructor(private api: ApiService) {}

  search(): void {
    this.error = '';
    const qs = this.employeeId ? `employeeId=${this.employeeId}` : '';
    if (!qs) {
      this.error = 'Enter an employee id.';
      return;
    }
    this.api.get<Payslip[]>(`payslips?${qs}`).subscribe({
      next: (p) => (this.payslips = p),
      error: (e) => (this.error = e.error ?? 'Failed to load payslips.')
    });
  }
}
