import { Component } from '@angular/core';
import { InsuredsListComponent } from '@features/insureds/components/insureds-list/insureds-list';

@Component({
  selector: 'app-insureds-page',
  standalone: true,
  imports: [InsuredsListComponent],
  template: '<app-insureds-list></app-insureds-list>'
})
export class InsuredsPageComponent {}
