import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  imports: [],
  template: `
    <div class="header-container">
      <p class="header-page-name">Hermes Dashboard</p>
    </div>
  `,
  styleUrl: './header.component.scss'
})

export class HeaderComponent { }
