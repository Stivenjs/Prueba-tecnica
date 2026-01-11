import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AseguradosService } from '../../../../core/services/asegurados.service';
import { Asegurado } from '../../../../shared/models/asegurado.model';

/**
 * Componente de lista/tabla de asegurados
 * Incluye búsqueda, paginación, edición y eliminación
 */
@Component({
  selector: 'app-asegurados-list',
  templateUrl: './asegurados-list.html',
  styleUrls: ['./asegurados-list.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule
  ]
})
export class AseguradosListComponent implements OnInit {
  private readonly aseguradosService = inject(AseguradosService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  displayedColumns: string[] = [
    'numeroIdentificacion',
    'nombreCompleto',
    'email',
    'telefono',
    'fechaNacimiento',
    'valorSolicitud',
    'acciones'
  ];

  asegurados: Asegurado[] = [];
  isLoading = false;
  searchTerm = '';
  
  // Paginación
  totalRecords = 0;
  pageSize = 10;
  currentPage = 1;
  pageSizeOptions = [5, 10, 20, 50];

  ngOnInit(): void {
    this.loadAsegurados();
  }

  /**
   * Cargar asegurados con paginación
   */
  loadAsegurados(): void {
    this.isLoading = true;
    this.aseguradosService.getAsegurados(this.currentPage, this.pageSize)
      .subscribe({
        next: (response) => {
          this.asegurados = response.data;
          this.totalRecords = response.totalRecords;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error al cargar asegurados:', error);
          this.showMessage('Error al cargar asegurados', 'error');
          this.isLoading = false;
        }
      });
  }

  /**
   * Buscar asegurados por número de identificación
   */
  onSearch(): void {
    if (!this.searchTerm || this.searchTerm.trim() === '') {
      this.loadAsegurados();
      return;
    }

    this.isLoading = true;
    this.aseguradosService.buscarPorIdentificacion(this.searchTerm)
      .subscribe({
        next: (asegurados) => {
          this.asegurados = asegurados;
          this.totalRecords = asegurados.length;
          this.isLoading = false;
          
          if (asegurados.length === 0) {
            this.showMessage('No se encontraron resultados', 'info');
          }
        },
        error: (error) => {
          console.error('Error en la búsqueda:', error);
          this.showMessage('Error en la búsqueda', 'error');
          this.isLoading = false;
        }
      });
  }

  /**
   * Limpiar búsqueda
   */
  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadAsegurados();
  }

  /**
   * Manejar cambio de página
   */
  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadAsegurados();
  }

  /**
   * Navegar a crear nuevo asegurado
   */
  onCreateNew(): void {
    this.router.navigate(['/asegurados/nuevo']);
  }

  /**
   * Editar asegurado
   */
  onEdit(asegurado: Asegurado): void {
    this.router.navigate(['/asegurados/editar', asegurado.numeroIdentificacion]);
  }

  /**
   * Eliminar asegurado con confirmación
   */
  onDelete(asegurado: Asegurado): void {
    const confirmacion = confirm(
      `¿Está seguro que desea eliminar al asegurado ${asegurado.primerNombre} ${asegurado.primerApellido}?`
    );

    if (!confirmacion) return;

    this.isLoading = true;
    this.aseguradosService.deleteAsegurado(asegurado.numeroIdentificacion)
      .subscribe({
        next: () => {
          this.showMessage('Asegurado eliminado exitosamente', 'success');
          this.loadAsegurados();
        },
        error: (error) => {
          console.error('Error al eliminar asegurado:', error);
          this.showMessage('Error al eliminar asegurado', 'error');
          this.isLoading = false;
        }
      });
  }

  /**
   * Obtener nombre completo
   */
  getNombreCompleto(asegurado: Asegurado): string {
    const segundo = asegurado.segundoNombre ? ` ${asegurado.segundoNombre}` : '';
    return `${asegurado.primerNombre}${segundo} ${asegurado.primerApellido} ${asegurado.segundoApellido}`;
  }

  /**
   * Formatear fecha
   */
  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-CO');
  }

  /**
   * Formatear moneda
   */
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(value);
  }

  /**
   * Mostrar mensaje
   */
  private showMessage(message: string, type: 'success' | 'error' | 'info'): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: [`snackbar-${type}`]
    });
  }
}
