import { Component } from '@angular/core';
import { AseguradosListComponent } from '../../components/asegurados-list/asegurados-list';

@Component({
  selector: 'app-asegurados-page',
  standalone: true,
  imports: [AseguradosListComponent],
  template: '<app-asegurados-list></app-asegurados-list>'
})
export class AseguradosPageComponent {}
