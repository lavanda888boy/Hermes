import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = environment.authServiceUrl;

  constructor(private http: HttpClient) { }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return (token !== '' && token !== null) ? true : false;
  }

  async login(user: User): Promise<void> {
    const token = await firstValueFrom(this.http.post(this.apiUrl, user, { responseType: 'text' }));
    localStorage.setItem('token', token);
  }
}
