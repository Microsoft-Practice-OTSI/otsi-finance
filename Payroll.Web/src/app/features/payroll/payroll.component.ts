import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/api.service';
import { CreatePayrollRun, PayrollRun } from '../../core/models';
import { PageHeaderComponent } from '../../shared/page-header.component';

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  template: `
    <app-page-header title="Payroll" subtitle="Run payroll and review past runs"></app-page-header>
    <div class="card">
      <h3>Run payroll</h3>
      <div class="form-row">
        <input [(ngModel)]="run.periodStart" type="date" />
        <input [(ngModel)]="run.periodEnd" type="date" />
        <input [(ngModel)]="run.payDate" type="date" />
        <button (click)="create()">Run</button>
      </div>
      <p class="error" *ngIf="error">{{ error }}</p>
      <p *ngIf="lastRun">Last run: {{ lastRun.payslipCount }} payslips, gross {{ lastRun.totalGross | number:'1.2-2' }}, net {{ lastRun.totalNet | number:'1.2-2' }}</p>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Run</th><th>Period</th><th>Payslips</th><th>Gross</th><th>Net</th><th>By</th></tr></thead>
        <tbody>
          <tr *ngFor="let r of runs">
            <td>#{{ r.id }}</td>
            <td>{{ r.periodStart | slice:0:10 }} - {{ r.periodEnd | slice:0:10 }}</td>
            <td>{{ r.payslipCount }}</td>
            <td>{{ r.totalGross | number:'1.2-2' }}</td>
            <td>{{ r.totalNet | number:'1.2-2' }}</td>
            <td>{{ r.createdBy }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `
})
export class PayrollComponent implements OnInit {
  runs: PayrollRun[] = [];
  run: CreatePayrollRun = { periodStart: '', periodEnd: '', payDate: '', departmentId: null };
  lastRun: PayrollRun | null = null;
  error = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.api.get<PayrollRun[]>('payroll').subscribe((r) => (this.runs = r));
  }

  create(): void {
    this.error = '';
    this.api.post<PayrollRun>('payroll', this.run).subscribe({
      next: (r) => {
        this.lastRun = r;
        this.load();
      },
      error: (e) => (this.error = e.error ?? 'Failed to run payroll.')
    });
  }
}
