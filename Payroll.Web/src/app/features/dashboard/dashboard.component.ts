import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../core/api.service';
import { Department, Employee } from '../../core/models';
import { PageHeaderComponent } from '../../shared/page-header.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, PageHeaderComponent],
  template: `
    <app-page-header title="Dashboard" subtitle="Your payroll workspace at a glance"></app-page-header>

    <div class="welcome animate__animated animate__fadeIn">
      <h2>Welcome back, Admin</h2>
    </div>

    <div class="tiles">
      <a class="tile animate__animated animate__fadeInUp" style="--accent:#2c5c8f; animation-delay:0ms"
         routerLink="/departments">
        <span class="ti-icon">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round">
            <path d="M3 21h18"/><path d="M5 21V7l7-4 7 4v14"/><path d="M9 21v-6h6v6"/><path d="M9 11h.01M15 11h.01"/>
          </svg>
        </span>
        <span class="ti-title">Departments</span>
        <span class="ti-desc">Manage organizational units</span>
        <span class="ti-stat" *ngIf="departmentCount !== null"><strong>{{ departmentCount }}</strong> departments</span>
      </a>

      <a class="tile animate__animated animate__fadeInUp" style="--accent:#1b9e8a; animation-delay:90ms"
         routerLink="/employees">
        <span class="ti-icon">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="9" cy="8" r="3.2"/><path d="M3.5 20a5.5 5.5 0 0 1 11 0"/><path d="M16 11a3 3 0 1 0 0-6"/><path d="M20.5 20a5.5 5.5 0 0 0-4-5.3"/>
          </svg>
        </span>
        <span class="ti-title">Employees</span>
        <span class="ti-desc">People, roles &amp; compensation</span>
        <span class="ti-stat" *ngIf="employeeCount !== null"><strong>{{ employeeCount }}</strong> employees</span>
      </a>

      <a class="tile animate__animated animate__fadeInUp" style="--accent:#b9761f; animation-delay:180ms"
         routerLink="/payroll">
        <span class="ti-icon">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round">
            <rect x="4" y="3" width="16" height="18" rx="2"/><circle cx="12" cy="13" r="4"/><path d="M12 9.5v3.5l2 1.5"/><path d="M8 3v2M16 3v2"/>
          </svg>
        </span>
        <span class="ti-title">Finance</span>
        <span class="ti-desc">Run &amp; review payroll</span>
        <span class="ti-stat">Process runs</span>
      </a>

      <a class="tile animate__animated animate__fadeInUp" style="--accent:#6b46c1; animation-delay:270ms"
         routerLink="/payslips">
        <span class="ti-icon">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7" stroke-linecap="round" stroke-linejoin="round">
            <path d="M6 2h9l4 4v16H6z"/><path d="M14 2v5h5"/><path d="M9 12h7M9 16h7M9 8h3"/>
          </svg>
        </span>
        <span class="ti-title">Payslips</span>
        <span class="ti-desc">View generated payslips</span>
        <span class="ti-stat">Search &amp; view</span>
      </a>
    </div>
  `,
  styles: [`
    .welcome { margin-bottom: 26px; }
    .welcome h2 { margin: 0; color: #1f2d3d; font-size: 24px; }

    .tiles {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(230px, 1fr));
      gap: 20px;
    }
    .tile {
      position: relative;
      display: flex;
      flex-direction: column;
      gap: 8px;
      padding: 24px;
      border-radius: 16px;
      background: #fff;
      box-shadow: 0 8px 24px rgba(20,40,70,0.08);
      text-decoration: none;
      color: #1f2d3d;
      overflow: hidden;
      transition: transform .28s cubic-bezier(.2,.7,.3,1), box-shadow .28s;
    }
    .tile::before {
      content: ""; position: absolute; left: 0; top: 0; right: 0; height: 4px;
      background: var(--accent); transform: scaleX(0); transform-origin: left; transition: transform .3s;
    }
    .tile:hover { transform: translateY(-10px); box-shadow: 0 26px 55px rgba(20,40,70,0.18); }
    .tile:hover::before { transform: scaleX(1); }
    .tile:hover .ti-icon { transform: scale(1.12) rotate(-5deg); }

    .ti-icon {
      width: 54px; height: 54px; border-radius: 14px;
      display: flex; align-items: center; justify-content: center;
      background: color-mix(in srgb, var(--accent) 12%, #fff);
      color: var(--accent);
      transition: transform .28s cubic-bezier(.2,.7,.3,1);
    }
    .ti-icon svg { width: 28px; height: 28px; }
    .ti-title { font-size: 18px; font-weight: 700; }
    .ti-desc { font-size: 13px; color: #6b7a8d; }
    .ti-stat { font-size: 13px; color: #41526a; margin-top: 2px; }
    .ti-stat strong { color: var(--accent); font-size: 15px; }

    @media (prefers-reduced-motion: reduce) {
      .tile, .ti-icon, .tile::before { transition: none; }
      .animate__animated { animation: none !important; }
    }
  `]
})
export class DashboardComponent implements OnInit {
  departmentCount: number | null = null;
  employeeCount: number | null = null;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.get<Department[]>('departments').subscribe({
      next: (d) => (this.departmentCount = d.length),
      error: () => (this.departmentCount = null)
    });
    this.api.get<Employee[]>('employees').subscribe({
      next: (e) => (this.employeeCount = e.length),
      error: () => (this.employeeCount = null)
    });
  }
}
