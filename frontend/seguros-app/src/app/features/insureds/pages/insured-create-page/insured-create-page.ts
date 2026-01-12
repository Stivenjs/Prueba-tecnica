import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { InsuredFormComponent } from '@features/insureds/components/insured-form/insured-form';
import { InsuredsService } from '@core/services/insureds.service';
import { CreateInsuredDto } from '@shared/models/insured.model';

@Component({
  selector: 'app-insured-create-page',
  standalone: true,
  imports: [InsuredFormComponent, MatSnackBarModule],
  templateUrl: './insured-create-page.html',
  styleUrls: ['./insured-create-page.css']
})
export class InsuredCreatePageComponent {
  private readonly insuredsService = inject(InsuredsService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  isLoading = signal(false);

  onSubmit(insured: CreateInsuredDto): void {
    this.isLoading.set(true);
    
    this.insuredsService.createInsured(insured).subscribe({
      next: () => {
        this.showMessage('Insured created successfully', 'success');
        this.router.navigate(['/insureds']);
      },
      error: (error) => {
        console.error('Error creating insured:', error);
        const message = error.error?.message || 'Error creating insured';
        this.showMessage(message, 'error');
        this.isLoading.set(false);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/insureds']);
  }

  private showMessage(message: string, type: 'success' | 'error'): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: [`snackbar-${type}`]
    });
  }
}
