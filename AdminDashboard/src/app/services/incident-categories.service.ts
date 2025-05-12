import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class IncidentCategoriesService {
  private apiUrl = environment.notificationCategoriesServiceUrl;

  private notificationCategoriesSource = new BehaviorSubject<string[]>([]);
  notificationCategories$ = this.notificationCategoriesSource.asObservable();

  constructor(private http: HttpClient) { }

  loadNotificationCategories(): void {
    this.http.get<string[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.notificationCategoriesSource.next(data);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  getNotificationCategories(): Observable<string[]> {
    return this.notificationCategories$;
  }
}
