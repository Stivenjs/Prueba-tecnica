import { Component, OnInit, inject, signal } from '@angular/core';
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
import { InsuredsService } from '@core/services/insureds.service';
import { Insured } from '@shared/models/insured.model';

/**
 * Componente de lista/tabla de asegurados
 * Incluye búsqueda, paginación, edición y eliminación
 */
@Component({
  selector: 'app-insureds-list',
  templateUrl: './insureds-list.html',
  styleUrls: ['./insureds-list.css'],
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
export class InsuredsListComponent implements OnInit {
  private readonly insuredsService = inject(InsuredsService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  displayedColumns: string[] = [
    'identificationNumber',
    'fullName',
    'email',
    'phone',
    'birthDate',
    'requestValue',
    'actions'
  ];

  // Usando Signals para reactividad automática
  insureds = signal<Insured[]>([]);
  isLoading = signal(false);
  searchTerm = signal('');
  
  // Paginación
  totalRecords = signal(0);
  pageSize = signal(10);
  currentPage = signal(1);
  pageSizeOptions = [5, 10, 20, 50];

  ngOnInit(): void {
    this.loadInsureds();
  }

  /**
   * Cargar asegurados con paginación
   */
  loadInsureds(): void {
    this.isLoading.set(true);
    this.insuredsService.getInsureds(this.currentPage(), this.pageSize())
      .subscribe({
        next: (response) => {
          this.insureds.set(response.data);
          this.totalRecords.set(response.totalRecords);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Error loading insureds:', error);
          this.showMessage('Error loading insureds', 'error');
          this.isLoading.set(false);
        }
      });
  }

  /**
   * Buscar asegurados por número de identificación
   */
  onSearch(): void {
    if (!this.searchTerm() || this.searchTerm().trim() === '') {
      this.loadInsureds();
      return;
    }

    this.isLoading.set(true);
    this.insuredsService.searchByIdentification(this.searchTerm())
      .subscribe({
        next: (response) => {
          this.insureds.set(response.results);
          this.totalRecords.set(response.totalCount);
          this.isLoading.set(false);
          
          // Mostrar mensaje informativo del backend
          if (response.message) {
            const type = response.totalCount === 0 ? 'info' : 'success';
            this.showMessage(response.message, type);
          }
        },
        error: (error) => {
          console.error('Search error:', error);
          this.showMessage('Search error', 'error');
          this.isLoading.set(false);
        }
      });
  }

  /**
   * Limpiar búsqueda
   */
  clearSearch(): void {
    this.searchTerm.set('');
    this.currentPage.set(1);
    this.loadInsureds();
  }

  /**
   * Manejar cambio de página
   */
  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.loadInsureds();
  }

  /**
   * Navegar a crear nuevo asegurado
   */
  onCreateNew(): void {
    this.router.navigate(['/insureds/new']);
  }

  /**
   * Editar asegurado
   */
  onEdit(insured: Insured): void {
    this.router.navigate(['/insureds/edit', insured.identificationNumber]);
  }

  /**
   * Eliminar asegurado con confirmación
   */
  onDelete(insured: Insured): void {
    const confirmation = confirm(
      `Are you sure you want to delete the insured ${insured.firstName} ${insured.firstLastName}?`
    );

    if (!confirmation) return;

    this.isLoading.set(true);
    this.insuredsService.deleteInsured(insured.identificationNumber)
      .subscribe({
        next: () => {
          this.showMessage('Insured deleted successfully', 'success');
          this.loadInsureds();
        },
        error: (error) => {
          console.error('Error deleting insured:', error);
          this.showMessage('Error deleting insured', 'error');
          this.isLoading.set(false);
        }
      });
  }

  /**
   * Obtener nombre completo
   */
  getFullName(insured: Insured): string {
    const middle = insured.middleName ? ` ${insured.middleName}` : '';
    return `${insured.firstName}${middle} ${insured.firstLastName} ${insured.secondLastName}`;
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
