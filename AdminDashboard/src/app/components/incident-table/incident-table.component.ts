import { Component, Input } from '@angular/core';
import { Incident } from '../../models/incident';
import { MatTableModule } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { CoordinatesPipe } from '../../pipes/coordinates.pipe';
import { SeverityPipe } from '../../pipes/severity.pipe';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-incident-table',
  imports: [
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
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

  constructor() { }

  getDisplayedColumns(): string[] {
    return this.columnVisibility === 'full'
      ? this.displayedColumns
      : this.displayedColumns.filter(col => !['severity', 'areaRadius'].includes(col));
  }
}
