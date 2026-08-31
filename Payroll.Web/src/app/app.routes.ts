import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';
import { LoginComponent } from './features/login/login.component';
import { ShellComponent } from './shell/shell.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { DepartmentsComponent } from './features/departments/departments.component';
import { EmployeesComponent } from './features/employees/employees.component';
import { PayrollComponent } from './features/payroll/payroll.component';
import { PayslipsComponent } from './features/payslips/payslips.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    canActivate: [authGuard],
    component: ShellComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'departments', component: DepartmentsComponent },
      { path: 'employees', component: EmployeesComponent },
      { path: 'payroll', component: PayrollComponent },
      { path: 'payslips', component: PayslipsComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];
