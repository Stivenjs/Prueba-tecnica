import { Routes } from '@angular/router';
import { AseguradosPageComponent } from './features/asegurados/pages/asegurados-page/asegurados-page';
import { AseguradoCreatePageComponent } from './features/asegurados/pages/asegurado-create-page/asegurado-create-page';
import { AseguradoEditPageComponent } from './features/asegurados/pages/asegurado-edit-page/asegurado-edit-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/asegurados',
    pathMatch: 'full'
  },
  {
    path: 'asegurados',
    component: AseguradosPageComponent,
    title: 'Gestión de Asegurados'
  },
  {
    path: 'asegurados/nuevo',
    component: AseguradoCreatePageComponent,
    title: 'Nuevo Asegurado'
  },
  {
    path: 'asegurados/editar/:id',
    component: AseguradoEditPageComponent,
    title: 'Editar Asegurado'
  },
  {
    path: '**',
    redirectTo: '/asegurados'
  }
];
