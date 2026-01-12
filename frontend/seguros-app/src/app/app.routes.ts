import { Routes } from '@angular/router';
import { InsuredsPageComponent } from '@features/insureds/pages/insureds-page/insureds-page';
import { InsuredCreatePageComponent } from '@features/insureds/pages/insured-create-page/insured-create-page';
import { InsuredEditPageComponent } from '@features/insureds/pages/insured-edit-page/insured-edit-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/insureds',
    pathMatch: 'full'
  },
  {
    path: 'insureds',
    component: InsuredsPageComponent,
    title: 'Gestión de Asegurados'
  },
  {
    path: 'insureds/new',
    component: InsuredCreatePageComponent,
    title: 'Nuevo Asegurado'
  },
  {
    path: 'insureds/edit/:id',
    component: InsuredEditPageComponent,
    title: 'Editar Asegurado'
  },
  {
    path: '**',
    redirectTo: '/insureds'
  }
];
