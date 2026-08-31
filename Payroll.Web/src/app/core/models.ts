export interface Department {
  id: number;
  name: string;
  code: string;
  description: string;
  createdDate: string;
}

export interface CreateDepartment {
  name: string;
  code: string;
  description: string;
}

export interface Employee {
  id: number;
  employeeId: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  jobTitle: string;
  departmentId: number;
  departmentName: string;
  payType: number;
  annualSalary: number;
  payRate: number;
  status: number;
  hireDate: string;
  createdDate: string;
}

export interface CreateEmployee {
  employeeId: string;
  firstName: string;
  lastName: string;
  email: string;
  jobTitle: string;
  departmentId: number;
  payType: number;
  annualSalary: number;
  payRate: number;
  status: number;
  hireDate: string;
}

export interface TimeEntry {
  id: number;
  employeeId: number;
  entryDate: string;
  regularHours: number;
  overtimeHours: number;
  description: string;
  createdDate: string;
}

export interface PayrollRun {
  id: number;
  periodStart: string;
  periodEnd: string;
  runDate: string;
  createdBy: string;
  status: number;
  totalGross: number;
  totalNet: number;
  payslipCount: number;
}

export interface CreatePayrollRun {
  periodStart: string;
  periodEnd: string;
  payDate: string;
  departmentId?: number | null;
}

export interface Payslip {
  id: number;
  employeeKey: number;
  employeeId: string;
  employeeName: string;
  regularPay: number;
  overtimePay: number;
  grossPay: number;
  taxDeduction: number;
  otherDeductions: number;
  netPay: number;
  payDate: string;
}

export interface LoginModel {
  username: string;
  password: string;
}

export interface TokenModel {
  token: string;
  expiresAt: string;
  username: string;
  roles: string[];
}
