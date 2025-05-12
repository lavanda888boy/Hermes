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
        skipNegotiation: true,
        transport: signalr.HttpTransportType.WebSockets,
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

  registerIncident(incident: Incident): void {
    const incidentRequest = {
      category: incident.category,
      severity: incident.severity,
      areaRadius: incident.areaRadius,
      longitude: incident.longitude,
      latitude: incident.latitude,
      description: incident.description
    };

    this.http.post(this.apiUrl, incidentRequest, { responseType: 'text' }).subscribe({
      next: () => {
        this.loadValidIncidents();
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  updateReportedIncident(incident: Incident): void {
    this.http.put(this.apiUrl, incident, { responseType: 'text' }).subscribe({
      next: () => {
        const currentIncidents = this.reportedIncidentsSource.getValue();
        const updatedReportedIncidents = currentIncidents.filter(inc => inc.id !== incident.id);

        this.reportedIncidentsSource.next(updatedReportedIncidents);
        this.validIncidentsSource.next([...this.validIncidentsSource.getValue(), incident]);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  updateValidIncident(incident: Incident): void {
    const params = new HttpParams().set('status', 'Update');

    this.http.put(this.apiUrl, incident, { params: params, responseType: 'text' }).subscribe({
      next: () => {
        const currentIncidents = this.validIncidentsSource.getValue();
        const updatedIncidents = currentIncidents.map(inc =>
          inc.id === incident.id ? { ...inc, ...incident } : inc
        );

        this.validIncidentsSource.next(updatedIncidents);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  deleteReportedIncident(id: string): void {
    this.http.delete(`${this.apiUrl}/${id}`, { responseType: 'text' }).subscribe({
      next: () => {
        const currentIncidents = this.reportedIncidentsSource.getValue();
        const updatedIncidents = currentIncidents.filter(incident => incident.id !== id);

        this.reportedIncidentsSource.next(updatedIncidents);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  deleteValidIncident(id: string): void {
    const params = new HttpParams().set('actor', 'Admin');

    this.http.delete(`${this.apiUrl}/${id}`, { params: params, responseType: 'text' }).subscribe({
      next: () => {
        const currentIncidents = this.validIncidentsSource.getValue();
        const updatedIncidents = currentIncidents.filter(incident => incident.id !== id);

        this.validIncidentsSource.next(updatedIncidents);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }
}
