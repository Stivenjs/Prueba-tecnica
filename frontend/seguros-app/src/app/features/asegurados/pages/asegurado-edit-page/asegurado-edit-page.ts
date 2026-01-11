import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { AseguradoFormComponent } from '@features/asegurados/components/asegurado-form/asegurado-form';
import { AseguradosService } from '@core/services/asegurados.service';
import { Asegurado, UpdateAseguradoDto } from '@shared/models/asegurado.model';

@Component({
  selector: 'app-asegurado-edit-page',
  standalone: true,
  imports: [
    CommonModule,
    AseguradoFormComponent,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './asegurado-edit-page.html',
  styleUrls: ['./asegurado-edit-page.css']
})
export class AseguradoEditPageComponent implements OnInit {
  private readonly aseguradosService = inject(AseguradosService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  asegurado = signal<Asegurado | undefined>(undefined);
  isLoading = signal(false);
  isLoadingData = signal(true);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadAsegurado(Number(id));
    } else {
      this.showMessage('ID no válido', 'error');
      this.router.navigate(['/asegurados']);
    }
  }

  private loadAsegurado(id: number): void {
    this.isLoadingData.set(true);
    
    this.aseguradosService.getAseguradoById(id).subscribe({
      next: (asegurado) => {
        this.asegurado.set(asegurado);
        this.isLoadingData.set(false);
      },
      error: (error) => {
        console.error('Error al cargar asegurado:', error);
        this.showMessage('Error al cargar asegurado', 'error');
        this.router.navigate(['/asegurados']);
      }
    });
  }

  onSubmit(aseguradoData: UpdateAseguradoDto): void {
    const currentAsegurado = this.asegurado();
    if (!currentAsegurado) return;

    this.isLoading.set(true);
    
    this.aseguradosService.updateAsegurado(currentAsegurado.numeroIdentificacion, aseguradoData)
      .subscribe({
        next: () => {
          this.showMessage('Asegurado actualizado exitosamente', 'success');
          this.router.navigate(['/asegurados']);
        },
        error: (error) => {
          console.error('Error al actualizar asegurado:', error);
          const mensaje = error.error?.message || 'Error al actualizar asegurado';
          this.showMessage(mensaje, 'error');
          this.isLoading.set(false);
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
