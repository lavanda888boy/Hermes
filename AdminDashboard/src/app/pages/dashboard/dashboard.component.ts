import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { MatTabsModule } from '@angular/material/tabs';
import { IncidentRegistrationService } from '../../services/incident-registration.service';
import { Incident } from '../../models/incident';
import { IncidentTableComponent } from '../../components/incident-table/incident-table.component';

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [
    HeaderComponent,
    IncidentTableComponent,
    MatTabsModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})

export class DashboardComponent implements OnInit {
  reportedIncidents: Incident[] = [];
  validIncidents: Incident[] = [];

  columnVisibility: string = 'partial';

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
        console.log('Valid Incidents:', this.validIncidents);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}
