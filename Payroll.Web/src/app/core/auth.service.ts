import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { LoginModel, TokenModel } from './models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'payroll_token';
  private authState = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private api: ApiService) {}

  login(model: LoginModel): Observable<TokenModel> {
    return new Observable<TokenModel>((observer) => {
      this.api.post<TokenModel>('auth/login', model).subscribe({
        next: (token) => {
          localStorage.setItem(this.tokenKey, token.token);
          this.authState.next(true);
          observer.next(token);
          observer.complete();
        },
        error: (err) => observer.error(err)
      });
    });
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.authState.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): Observable<boolean> {
    return this.authState.asObservable();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
}
