import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Incident } from '../models/incident';
import { BehaviorSubject, Observable } from 'rxjs';
import * as signalr from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})

export class IncidentRegistrationService {
  private apiUrl = environment.incidentRegistrationServiceUrl;
  private hubUrl = environment.incidentHubUrl;

  private reportedIncidentsSource = new BehaviorSubject<Incident[]>([]);
  reportedIncidents$ = this.reportedIncidentsSource.asObservable();

  private validIncidentsSource = new BehaviorSubject<Incident[]>([]);
  validIncidents$ = this.validIncidentsSource.asObservable();

  private hubConnection!: signalr.HubConnection;

  constructor(private http: HttpClient) {
    this.startSignalRConnection();
  }

  private startSignalRConnection(): void {
    this.hubConnection = new signalr.HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        transport: signalr.HttpTransportType.ServerSentEvents,
        accessTokenFactory: () => localStorage.getItem('token') || ''
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR connection error:', err));

    this.hubConnection.on('ReceiveIncident', (incident: Incident) => {
      const current = this.reportedIncidentsSource.value;
      this.reportedIncidentsSource.next([...current, incident]);
    });
  }

  loadReportedIncidents(): void {
    const params = new HttpParams().set('status', 'Pending');

    this.http.get<Incident[]>(this.apiUrl, { params: params }).subscribe({
      next: (data) => {
        this.reportedIncidentsSource.next(data);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  getReportedIncidents(): Observable<Incident[]> {
    return this.reportedIncidents$;
  }

  loadValidIncidents(): void {
    const params = new HttpParams().set('status', 'Approved');

    this.http.get<Incident[]>(this.apiUrl, { params: params }).subscribe({
      next: (data) => {
        this.validIncidentsSource.next(data);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  getValidIncidents(): Observable<Incident[]> {
    return this.validIncidents$;
  }
}
