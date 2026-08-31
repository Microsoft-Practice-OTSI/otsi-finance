import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <header class="app-header">
      <a class="brand" routerLink="/dashboard">
        <img src="assets/logo/o_fin_logo.png" alt="OFin logo" class="brand-logo" />
        OFin
      </a>
      <span class="spacer"></span>
      <button class="secondary" (click)="logout()">Sign out</button>
    </header>
    <main class="content">
      <router-outlet></router-outlet>
    </main>
  `
})
export class ShellComponent {
  constructor(private auth: AuthService) {}

  logout(): void {
    this.auth.logout();
    location.href = '/login';
  }
}
