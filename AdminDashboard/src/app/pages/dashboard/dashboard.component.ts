import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { MatTabsModule } from '@angular/material/tabs';
import { IncidentRegistrationService } from '../../services/incident-registration.service';
import { Incident } from '../../models/incident';
import { IncidentTableComponent } from '../../components/incident-table/incident-table.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { IncidentFormComponent } from '../../components/incident-form/incident-form.component';

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [
    HeaderComponent,
    IncidentTableComponent,
    MatTabsModule,
    MatDialogModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})

export class DashboardComponent implements OnInit {
  reportedIncidents: Incident[] = [];
  validIncidents: Incident[] = [];

  columnVisibility: string = 'partial';

  constructor(
    private incidentRegistrationService: IncidentRegistrationService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.incidentRegistrationService.loadReportedIncidents();
    this.incidentRegistrationService.loadValidIncidents();

    this.incidentRegistrationService.getReportedIncidents().subscribe({
      next: (data) => {
        this.reportedIncidents = data;
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.incidentRegistrationService.getValidIncidents().subscribe({
      next: (data) => {
        this.validIncidents = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  openIncidentReportDialog(): void {
    const dialogRef = this.dialog.open(IncidentFormComponent, {
      width: "600px",
      height: "550px",
      data: undefined
    });

    dialogRef.afterClosed().subscribe((result: Incident | null) => {
      if (result) {
        this.incidentRegistrationService.registerIncident(result);
      }
    });
  }
}
