import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { 
  FormBuilder, 
  FormGroup, 
  ReactiveFormsModule, 
  Validators 
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Insured, CreateInsuredDto } from '@shared/models/insured.model';

/**
 * Componente de formulario para crear/editar asegurados
 * Utiliza Reactive Forms para un control total y validaciones robustas
 */
@Component({
  selector: 'app-insured-form',
  templateUrl: './insured-form.html',
  styleUrls: ['./insured-form.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatProgressSpinnerModule
  ]
})
export class InsuredFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);

  @Input() insured?: Insured; // Para edición
  @Input() isLoading = false;
  @Output() submitForm = new EventEmitter<CreateInsuredDto>();
  @Output() cancelForm = new EventEmitter<void>();

  insuredForm!: FormGroup;
  isEditMode = false;
  maxDate = new Date(); // No permitir fechas futuras

  ngOnInit(): void {
    this.initForm();
    
    if (this.insured) {
      this.isEditMode = true;
      this.patchFormValues();
    }
  }

  /**
   * Inicializar el formulario con validaciones
   */
  private initForm(): void {
    this.insuredForm = this.fb.group({
      identificationNumber: [
        { value: '', disabled: this.isEditMode }, 
        [
          Validators.required,
          Validators.min(1),
          Validators.pattern('^[0-9]+$')
        ]
      ],
      firstName: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      middleName: [
        '', 
        [
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      firstLastName: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      secondLastName: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      contactPhone: [
        '', 
        [
          Validators.required,
          Validators.minLength(7),
          Validators.maxLength(20),
          Validators.pattern('^[0-9+\\-\\s()]+$')
        ]
      ],
      email: [
        '', 
        [
          Validators.required,
          Validators.email,
          Validators.maxLength(100)
        ]
      ],
      birthDate: [
        '', 
        [
          Validators.required,
          this.validateMinimumAge(18)
        ]
      ],
      estimatedRequestValue: [
        '', 
        [
          Validators.required,
          Validators.min(0.01),
          Validators.pattern('^[0-9]+(\.[0-9]{1,2})?$')
        ]
      ],
      observations: [
        '', 
        [Validators.maxLength(500)]
      ]
    });
  }

  /**
   * Rellenar el formulario con datos del asegurado (modo edición)
   */
  private patchFormValues(): void {
    if (!this.insured) return;

    this.insuredForm.patchValue({
      identificationNumber: this.insured.identificationNumber,
      firstName: this.insured.firstName,
      middleName: this.insured.middleName || '',
      firstLastName: this.insured.firstLastName,
      secondLastName: this.insured.secondLastName,
      contactPhone: this.insured.contactPhone,
      email: this.insured.email,
      birthDate: new Date(this.insured.birthDate),
      estimatedRequestValue: this.insured.estimatedRequestValue,
      observations: this.insured.observations || ''
    });
  }

  /**
   * Validador personalizado: edad mínima
   */
  private validateMinimumAge(minimumAge: number) {
    return (control: any) => {
      if (!control.value) return null;

      const birthDate = new Date(control.value);
      const today = new Date();
      let age = today.getFullYear() - birthDate.getFullYear();
      const month = today.getMonth() - birthDate.getMonth();
      
      if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }

      return age >= minimumAge ? null : { minimumAge: { required: minimumAge, actual: age } };
    };
  }

  /**
   * Obtener mensaje de error para un campo
   */
  getErrorMessage(fieldName: string): string {
    const control = this.insuredForm.get(fieldName);
    if (!control || !control.errors) return '';

    if (control.hasError('required')) return 'Este campo es requerido';
    if (control.hasError('email')) return 'Email inválido';
    if (control.hasError('min')) return `Valor mínimo: ${control.errors['min'].min}`;
    if (control.hasError('minlength')) return `Mínimo ${control.errors['minlength'].requiredLength} caracteres`;
    if (control.hasError('maxlength')) return `Máximo ${control.errors['maxlength'].requiredLength} caracteres`;
    if (control.hasError('pattern')) return 'Formato inválido';
    if (control.hasError('minimumAge')) return `Edad mínima: ${control.errors['minimumAge'].required} años`;

    return 'Campo inválido';
  }

  /**
   * Enviar formulario
   */
  onSubmit(): void {
    if (this.insuredForm.invalid) {
      this.insuredForm.markAllAsTouched();
      return;
    }

    const formValue = this.insuredForm.getRawValue();
    
    // Formatear fecha a ISO 8601
    const birthDate = new Date(formValue.birthDate);
    birthDate.setHours(0, 0, 0, 0);
    
    const insuredData: CreateInsuredDto = {
      ...formValue,
      birthDate: birthDate.toISOString(),
      identificationNumber: Number(formValue.identificationNumber),
      estimatedRequestValue: Number(formValue.estimatedRequestValue)
    };

    this.submitForm.emit(insuredData);
  }

  /**
   * Cancelar formulario
   */
  onCancel(): void {
    this.cancelForm.emit();
  }

  /**
   * Resetear formulario
   */
  resetForm(): void {
    this.insuredForm.reset();
  }
}
