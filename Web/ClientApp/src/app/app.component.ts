import { Component } from '@angular/core';
import packageJson from '../../package.json';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor() {
    var version = packageJson.version;
    console.log(`Contemplation slideshow by Dem, version ${version}`);
  }
}
