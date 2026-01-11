import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AseguradoFormComponent } from '../../components/asegurado-form/asegurado-form';
import { AseguradosService } from '../../../../core/services/asegurados.service';
import { CreateAseguradoDto } from '../../../../shared/models/asegurado.model';

@Component({
  selector: 'app-asegurado-create-page',
  standalone: true,
  imports: [AseguradoFormComponent, MatSnackBarModule],
  templateUrl: './asegurado-create-page.html',
  styleUrls: ['./asegurado-create-page.css']
})
export class AseguradoCreatePageComponent {
  private readonly aseguradosService = inject(AseguradosService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  isLoading = false;

  onSubmit(asegurado: CreateAseguradoDto): void {
    this.isLoading = true;
    
    this.aseguradosService.createAsegurado(asegurado).subscribe({
      next: () => {
        this.showMessage('Asegurado creado exitosamente', 'success');
        this.router.navigate(['/asegurados']);
      },
      error: (error) => {
        console.error('Error al crear asegurado:', error);
        const mensaje = error.error?.message || 'Error al crear asegurado';
        this.showMessage(mensaje, 'error');
        this.isLoading = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/asegurados']);
  }

  private showMessage(message: string, type: 'success' | 'error'): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: [`snackbar-${type}`]
    });
  }
}
