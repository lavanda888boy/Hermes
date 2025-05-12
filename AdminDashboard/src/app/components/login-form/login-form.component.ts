import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { User } from '../../models/user';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-form',
  imports: [
    NgIf,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss'
})

export class LoginFormComponent {
  loginForm = new FormGroup({
    name: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  async handleSubmit(): Promise<void> {
    const user: User = {
      userName: this.loginForm.value.name!,
      password: this.loginForm.value.password!
    };

    try {
      await this.authService.login(user);
      this.router.navigate(['/'], { replaceUrl: true });
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        let snackBarMessage = '';

        if (error.status === 400) {
          snackBarMessage = '⛔ Login failed. Invalid credentials.';
        } else {
          snackBarMessage = '⛔ Something went wrong. Please try again later.';
        }

        this.snackBar.open(snackBarMessage, 'Close', {
          duration: 3000
        });

        console.log('Error:', error);
      }

      this.loginForm.reset();
    }
  }
}
