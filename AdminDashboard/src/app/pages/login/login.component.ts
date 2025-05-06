import { Component } from '@angular/core';
import { LoginFormComponent } from '../../login-form/login-form.component';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [LoginFormComponent],
  template: `
    <div class="login-page-wrapper">
      <app-login-form />
    </div>
  `,
  styleUrl: './login.component.scss'
})

export class LoginComponent { }
