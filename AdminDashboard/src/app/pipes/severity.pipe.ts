import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'severity'
})

export class SeverityPipe implements PipeTransform {
  transform(severity: string): string {
    let output = '';

    switch (severity) {
      case 'LOW':
        output = '🟢';
        break;
      case 'MODERATE':
        output = '🟡';
        break;
      case 'HIGH':
        output = '🔴';
        break;
      default:
        output = 'No clasificada';
    }

    return output;
  }
}
