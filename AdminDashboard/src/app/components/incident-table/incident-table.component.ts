import { Component, Input } from '@angular/core';
import { Incident } from '../../models/incident';
import { MatTableModule } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { CoordinatesPipe } from '../../pipes/coordinates.pipe';
import { SeverityPipe } from '../../pipes/severity.pipe';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { IncidentRegistrationService } from '../../services/incident-registration.service';
import { IncidentFormComponent } from '../incident-form/incident-form.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-incident-table',
  imports: [
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatDialogModule,
    MatTooltipModule,
    DatePipe,
    SeverityPipe,
    CoordinatesPipe
  ],
  templateUrl: './incident-table.component.html',
  styleUrl: './incident-table.component.scss'
})

export class IncidentTableComponent {
  @Input() incidents: Incident[] = [];
  @Input() columnVisibility: string = 'full';

  selectedIncident: Incident | null = null;

  displayedColumns: string[] = [
    'category',
    'severity',
    'areaRadius',
    'timestamp',
    'coordinates',
    'userToReport',
    'description',
    'actions'
  ];

  constructor(
    private incidentRegistrationService: IncidentRegistrationService,
    private dialog: MatDialog
  ) { }

  getDisplayedColumns(): string[] {
    return this.columnVisibility === 'full'
      ? this.displayedColumns
      : this.displayedColumns.filter(col => !['severity', 'areaRadius'].includes(col));
  }

  setSelectedIncident(incident: Incident): void {
    this.selectedIncident = incident;
  }

  openIncidentReportDialog(): void {
    const dialogRef = this.dialog.open(IncidentFormComponent, {
      width: "600px",
      height: "550px",
      data: this.selectedIncident
    });

    dialogRef.afterClosed().subscribe((result: Incident | null) => {
      if (result) {
        if (this.columnVisibility === 'full') {
          this.incidentRegistrationService.updateValidIncident(result);
        } else {
          this.incidentRegistrationService.updateReportedIncident(result);
        }
      }
    });
  }

  handleDeleteIncident(): void {
    if (this.columnVisibility === 'full') {
      this.incidentRegistrationService.deleteValidIncident(this.selectedIncident!.id);
    } else {
      this.incidentRegistrationService.deleteReportedIncident(this.selectedIncident!.id);
    }
  }
}
