import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Incident } from '../../models/incident';
import { MatSelectModule } from '@angular/material/select';
import { NgFor, NgIf } from '@angular/common';
import { IncidentCategoriesService } from '../../services/incident-categories.service';

@Component({
  selector: 'app-incident-form',
  imports: [
    NgIf,
    NgFor,
    MatDialogModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ],
  templateUrl: './incident-form.component.html',
  styleUrl: './incident-form.component.scss'
})

export class IncidentFormComponent implements OnInit {
  incidentForm = new FormGroup({
    category: new FormControl('', Validators.required),
    severity: new FormControl('', Validators.required),
    areaRadius: new FormControl(0.1, [
      Validators.required,
      Validators.min(0.1)
    ]),
    longitude: new FormControl(0, Validators.required),
    latitude: new FormControl(0, Validators.required),
    description: new FormControl('', [Validators.maxLength(1000)])
  });

  categories: string[] = [];
  severities: string[] = ['LOW', 'MODERATE', 'HIGH'];

  constructor(
    private dialogRef: MatDialogRef<IncidentFormComponent>,
    private incidentCategoriesService: IncidentCategoriesService,
    @Inject(MAT_DIALOG_DATA) public incident: Incident | undefined,
  ) { }

  ngOnInit() {
    this.incidentCategoriesService.loadNotificationCategories();

    this.incidentCategoriesService.getNotificationCategories().subscribe({
      next: (data) => {
        this.categories = data;

        this.incidentForm.patchValue({
          category: this.categories[0],
          severity: this.severities[0]
        });

        if (this.incident) {
          this.incidentForm.patchValue({
            category: this.incident.category,
            severity: this.incident.severity,
            areaRadius: this.incident.areaRadius,
            longitude: this.incident.longitude,
            latitude: this.incident.latitude,
            description: this.incident.description
          });
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  handleSubmit() {
    const formValue = this.incidentForm.value;

    const newIncident: Incident = {
      id: this.incident ? this.incident.id : '',
      category: formValue.category!,
      severity: formValue.severity!,
      areaRadius: formValue.areaRadius!,
      timestamp: '',
      longitude: formValue.longitude!,
      latitude: formValue.latitude!,
      userToReport: '',
      status: '',
      description: formValue.description!
    };

    this.dialogRef.close(newIncident);
  }
}
