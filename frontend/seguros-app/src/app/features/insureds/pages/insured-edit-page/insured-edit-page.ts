import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { InsuredFormComponent } from '@features/insureds/components/insured-form/insured-form';
import { InsuredsService } from '@core/services/insureds.service';
import { Insured, UpdateInsuredDto } from '@shared/models/insured.model';

@Component({
  selector: 'app-insured-edit-page',
  standalone: true,
  imports: [
    CommonModule,
    InsuredFormComponent,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './insured-edit-page.html',
  styleUrls: ['./insured-edit-page.css']
})
export class InsuredEditPageComponent implements OnInit {
  private readonly insuredsService = inject(InsuredsService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  insured = signal<Insured | undefined>(undefined);
  isLoading = signal(false);
  isLoadingData = signal(true);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadInsured(Number(id));
    } else {
      this.showMessage('Invalid ID', 'error');
      this.router.navigate(['/insureds']);
    }
  }

  private loadInsured(id: number): void {
    this.isLoadingData.set(true);
    
    this.insuredsService.getInsuredById(id).subscribe({
      next: (insured) => {
        this.insured.set(insured);
        this.isLoadingData.set(false);
      },
      error: (error) => {
        console.error('Error loading insured:', error);
        this.showMessage('Error loading insured', 'error');
        this.router.navigate(['/insureds']);
      }
    });
  }

  onSubmit(insuredData: UpdateInsuredDto): void {
    const currentInsured = this.insured();
    if (!currentInsured) return;

    this.isLoading.set(true);
    
    this.insuredsService.updateInsured(currentInsured.identificationNumber, insuredData)
      .subscribe({
        next: () => {
          this.showMessage('Insured updated successfully', 'success');
          this.router.navigate(['/insureds']);
        },
        error: (error) => {
          console.error('Error updating insured:', error);
          const message = error.error?.message || 'Error updating insured';
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
