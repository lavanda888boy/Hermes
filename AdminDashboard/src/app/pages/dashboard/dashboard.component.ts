import { Component, OnDestroy, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { MatTabsModule } from '@angular/material/tabs';
import { IncidentRegistrationService } from '../../services/incident-registration.service';
import { Incident } from '../../models/incident';

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [
    HeaderComponent,
    MatTabsModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})

export class DashboardComponent implements OnInit {
  reportedIncidents: Incident[] = [];
  validIncidents: Incident[] = [];

  constructor(private incidentRegistrationService: IncidentRegistrationService) { }

  ngOnInit(): void {
    this.incidentRegistrationService.getReportedIncidents();
    this.incidentRegistrationService.loadValidIncidents();

    this.incidentRegistrationService.reportedIncidents$.subscribe({
      next: (data) => {
        this.reportedIncidents = data;
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.incidentRegistrationService.validIncidents$.subscribe({
      next: (data) => {
        this.validIncidents = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}
