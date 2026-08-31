import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-header">
      <div class="ph-text">
        <h1>{{ title }}</h1>
        <p *ngIf="subtitle" class="sub">{{ subtitle }}</p>
      </div>
    </div>
  `,
  styles: [`
    .page-header {
      display: flex;
      align-items: center;
      margin-bottom: 22px;
    }
    .ph-text h1 {
      margin: 0;
      font-size: 26px;
      color: #1f2d3d;
      letter-spacing: -0.3px;
    }
    .ph-text .sub {
      margin: 4px 0 0;
      color: #6b7a8d;
      font-size: 14px;
    }
  `]
})
export class PageHeaderComponent {
  @Input() title = '';
  @Input() subtitle = '';
}
