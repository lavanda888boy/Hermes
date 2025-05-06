import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class IncidentRegistrationService {
  private apiUrl = environment.incidentRegistrationServiceUrl;

  constructor(private http: HttpClient) { }
}
