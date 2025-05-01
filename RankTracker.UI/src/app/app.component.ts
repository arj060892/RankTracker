import { Component } from '@angular/core';
import { Observable, interval } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  currentDateTime: Date = new Date();
  positions: number[] = [1, 2, 3];

  constructor() {
    // Update time every minute (60000 ms)
    setInterval(() => {
      this.currentDateTime = new Date();
    }, 60000);
  }
}
