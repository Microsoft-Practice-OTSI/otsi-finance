import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { animate, keyframes, style, trigger, transition } from '@angular/animations';
import { AuthService } from '../../core/auth.service';
import { LoginModel } from '../../core/models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  animations: [
    trigger('fadeUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(28px)' }),
        animate('650ms cubic-bezier(.2,.7,.3,1)', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ]),
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('900ms ease-out', style({ opacity: 1 }))
      ])
    ]),
    trigger('floatSoft', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(18px) scale(.98)' }),
        animate('1000ms 200ms cubic-bezier(.2,.7,.3,1)', style({ opacity: 1, transform: 'translateY(0) scale(1)' }))
      ])
    ]),
    trigger('shake', [
      transition('* => error', [
        animate('450ms', keyframes([
          style({ transform: 'translateX(0)' }),
          style({ transform: 'translateX(-9px)' }),
          style({ transform: 'translateX(8px)' }),
          style({ transform: 'translateX(-6px)' }),
          style({ transform: 'translateX(5px)' }),
          style({ transform: 'translateX(0)' })
        ]))
      ])
    ])
  ],
  template: `
    <div class="login">
      <section class="brand" @fadeIn>
        <div class="mesh"></div>
        <div class="orb orb-1"></div>
        <div class="orb orb-2"></div>
        <div class="orb orb-3"></div>

        <div class="brand-content">
          <div class="brand-top">
            <img class="logo" src="assets/logo/o_fin_logo.png" alt="OFin" @floatSoft />
            <span class="brand-name">OFin</span>
          </div>

          <div class="brand-mid">
            <h1 class="headline">Payroll,<br />reimagined.</h1>
            <p class="sub">The modern workspace to manage people, run payroll, and deliver payslips — all in one place.</p>
            <ul class="features">
              <li><span class="dot"></span> Manage employees &amp; departments</li>
              <li><span class="dot"></span> Run accurate, auditable payroll</li>
              <li><span class="dot"></span> Distribute payslips in seconds</li>
            </ul>
          </div>

          <div class="brand-foot">
            <span class="copyright">&copy; {{ year }} OFin. All rights reserved.</span>
          </div>
        </div>
      </section>

      <section class="panel">
        <div class="card" @fadeUp [@shake]="animState">
          <h2>Welcome back</h2>
          <p class="muted">Sign in to your account to continue</p>
          <form (ngSubmit)="submit()">
            <div class="field">
              <input id="username" [(ngModel)]="model.username" name="username" placeholder=" " (input)="resetAnim()" autocomplete="username" required />
              <label for="username">Username</label>
            </div>
            <div class="field">
              <input id="password" [(ngModel)]="model.password" name="password" type="password" placeholder=" " (input)="resetAnim()" autocomplete="current-password" required />
              <label for="password">Password</label>
            </div>
            <div class="row">
              <label class="remember"><input type="checkbox" /> Remember me</label>
              <a class="link" href="#">Forgot password?</a>
            </div>
            <button type="submit" class="submit">Sign in</button>
          </form>
          <p class="error" *ngIf="error">{{ error }}</p>
          <p class="secure"><span class="lock"></span> Secured with end-to-end encryption</p>
        </div>
      </section>
    </div>
  `,
  styles: [`
    :host { display: block; }
    .login {
      min-height: 100vh;
      display: grid;
      grid-template-columns: 1.05fr 1fr;
      font-family: 'Inter', 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
    }

    .brand {
      position: relative;
      overflow: hidden;
      background: radial-gradient(120% 120% at 0% 0%, #122a4f 0%, #0b1c38 50%, #0a1428 100%);
      color: #fff;
      display: flex;
      flex-direction: column;
      justify-content: center;
      padding: 64px;
    }
    .mesh {
      position: absolute;
      inset: -20%;
      background:
        radial-gradient(40% 40% at 20% 30%, rgba(110,168,254,0.35), transparent 60%),
        radial-gradient(35% 35% at 80% 20%, rgba(139,92,246,0.30), transparent 60%),
        radial-gradient(45% 45% at 70% 80%, rgba(56,189,248,0.25), transparent 60%);
      filter: blur(20px);
      animation: drift 18s ease-in-out infinite alternate;
      z-index: 0;
    }
    @keyframes drift {
      0% { transform: translate3d(0,0,0) scale(1); }
      100% { transform: translate3d(2%, -2%, 0) scale(1.08); }
    }
    .orb {
      position: absolute;
      border-radius: 50%;
      filter: blur(40px);
      opacity: .5;
      z-index: 0;
      animation: floaty 10s ease-in-out infinite;
    }
    .orb-1 { width: 280px; height: 280px; background: #3b82f6; top: -70px; right: -40px; }
    .orb-2 { width: 220px; height: 220px; background: #8b5cf6; bottom: -60px; left: 10%; animation-delay: 2s; }
    .orb-3 { width: 160px; height: 160px; background: #22d3ee; top: 40%; left: 40%; opacity: .35; animation-delay: 4s; }
    @keyframes floaty {
      0%, 100% { transform: translateY(0); }
      50% { transform: translateY(30px); }
    }

    .brand-content {
      position: relative;
      z-index: 2;
      display: flex;
      flex-direction: column;
      height: 100%;
      max-width: 460px;
    }
    .brand-top { display: flex; align-items: center; gap: 14px; }
    .logo { width: 54px; height: 54px; border-radius: 14px; box-shadow: 0 10px 30px rgba(0,0,0,0.35); }
    .brand-name { font-size: 22px; font-weight: 800; letter-spacing: .5px; }
    .brand-mid { margin: auto 0; padding: 48px 0; }
    .headline {
      font-size: 54px; line-height: 1.05; font-weight: 800; letter-spacing: -1px; margin: 0 0 18px;
      background: linear-gradient(120deg, #ffffff, #9ec5ff);
      -webkit-background-clip: text; background-clip: text; color: transparent;
    }
    .sub { color: #b9c7df; font-size: 16px; line-height: 1.6; max-width: 380px; margin: 0 0 28px; }
    .features { list-style: none; padding: 0; margin: 0; display: grid; gap: 14px; }
    .features li { display: flex; align-items: center; gap: 12px; color: #e6eef8; font-size: 15px; }
    .dot {
      width: 10px; height: 10px; border-radius: 50%;
      background: linear-gradient(135deg, #6ea8fe, #8b5cf6);
      box-shadow: 0 0 0 4px rgba(110,168,254,0.15);
    }
    .brand-foot { font-size: 12px; color: #8aa0c2; }

    .panel {
      display: flex; align-items: center; justify-content: center;
      background: linear-gradient(160deg, #f6f8fc, #eef2f8);
      padding: 24px;
    }
    .card {
      width: 100%; max-width: 400px; padding: 40px 36px;
      background: rgba(255,255,255,0.85);
      border: 1px solid rgba(255,255,255,0.6);
      border-radius: 24px;
      box-shadow: 0 24px 60px rgba(15,30,60,0.14), inset 0 1px 0 rgba(255,255,255,0.8);
      backdrop-filter: blur(14px);
    }
    .card-logo { width: 48px; height: 48px; border-radius: 12px; margin-bottom: 18px; box-shadow: 0 8px 20px rgba(15,30,60,0.12); }
    .card h2 { margin: 0; font-size: 26px; color: #0f1f38; font-weight: 800; letter-spacing: -0.5px; }
    .muted { color: #64748b; margin: 8px 0 26px; font-size: 14px; }

    .field { position: relative; margin-bottom: 18px; }
    .field input {
      width: 100%; height: 56px; padding: 18px 16px 8px;
      border: 1.5px solid #e2e8f0; border-radius: 14px; background: #fff;
      font-size: 15px; color: #0f1f38;
      transition: border-color .2s, box-shadow .2s, background .2s;
    }
    .field input:focus { outline: none; border-color: #6ea8fe; box-shadow: 0 0 0 4px rgba(110,168,254,0.18); }
    .field label {
      position: absolute; left: 16px; top: 18px; color: #94a3b8; font-size: 15px;
      pointer-events: none; transition: all .18s ease; padding: 0 4px;
    }
    .field input:focus + label,
    .field input:not(:placeholder-shown) + label {
      top: 8px; font-size: 11px; font-weight: 600; color: #6ea8fe; background: #fff;
    }
    .row { display: flex; align-items: center; justify-content: space-between; margin: 4px 2px 22px; font-size: 13px; }
    .remember { display: flex; align-items: center; gap: 8px; color: #475569; cursor: pointer; }
    .remember input { width: 16px; height: 16px; accent-color: #6ea8fe; }
    .link { color: #3b6fd6; text-decoration: none; font-weight: 600; }
    .link:hover { text-decoration: underline; }

    .submit {
      width: 100%; padding: 15px; border: none; border-radius: 14px; color: #fff; font-size: 15px; font-weight: 700; cursor: pointer;
      background: linear-gradient(135deg, #3b6fd6, #6d4bdc); background-size: 160% 160%;
      box-shadow: 0 12px 26px rgba(59,111,214,0.35);
      transition: transform .12s, box-shadow .2s, background-position .4s;
    }
    .submit:hover { transform: translateY(-2px); box-shadow: 0 16px 34px rgba(59,111,214,0.45); background-position: 100% 0; }
    .submit:active { transform: translateY(0); }

    .error { color: #e11d48; font-size: 13px; margin-top: 16px; text-align: center; }
    .secure { margin-top: 20px; font-size: 12px; color: #94a3b8; text-align: center; display: flex; align-items: center; justify-content: center; gap: 6px; }
    .lock { width: 12px; height: 12px; border-radius: 3px; border: 1.5px solid #94a3b8; position: relative; }
    .lock::after {
      content: ""; position: absolute; top: -6px; left: 2px; width: 6px; height: 6px;
      border: 1.5px solid #94a3b8; border-bottom: none; border-radius: 6px 6px 0 0;
    }

    @media (max-width: 880px) {
      .login { grid-template-columns: 1fr; }
      .brand { display: none; }
    }
  `]
})
export class LoginComponent {
  model: LoginModel = { username: '', password: '' };
  error = '';
  animState = 'normal';
  year = new Date().getFullYear();

  constructor(private auth: AuthService, private router: Router) {}

  resetAnim(): void {
    this.animState = 'normal';
  }

  submit(): void {
    this.error = '';
    this.auth.login(this.model).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: () => {
        this.error = 'Invalid username or password.';
        this.animState = 'normal';
        setTimeout(() => (this.animState = 'error'), 20);
      }
    });
  }
}
