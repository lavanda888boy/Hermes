import { Pipe, PipeTransform } from '@angular/core';
import { Incident } from '../models/incident';

@Pipe({
  name: 'coordinates'
})

export class CoordinatesPipe implements PipeTransform {
  transform(incident: Incident): string {
    return `LON: ${incident.longitude.toFixed(4)}, LAT: ${incident.latitude.toFixed(4)}`;
  }
}
