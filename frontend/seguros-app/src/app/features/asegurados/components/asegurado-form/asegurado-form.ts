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
import { Asegurado, CreateAseguradoDto } from '@shared/models/asegurado.model';

/**
 * Componente de formulario para crear/editar asegurados
 * Utiliza Reactive Forms para un control total y validaciones robustas
 */
@Component({
  selector: 'app-asegurado-form',
  templateUrl: './asegurado-form.html',
  styleUrls: ['./asegurado-form.css'],
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
export class AseguradoFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);

  @Input() asegurado?: Asegurado; // Para edición
  @Input() isLoading = false;
  @Output() submitForm = new EventEmitter<CreateAseguradoDto>();
  @Output() cancelForm = new EventEmitter<void>();

  aseguradoForm!: FormGroup;
  isEditMode = false;
  maxDate = new Date(); // No permitir fechas futuras

  ngOnInit(): void {
    this.initForm();
    
    if (this.asegurado) {
      this.isEditMode = true;
      this.patchFormValues();
    }
  }

  /**
   * Inicializar el formulario con validaciones
   */
  private initForm(): void {
    this.aseguradoForm = this.fb.group({
      numeroIdentificacion: [
        { value: '', disabled: this.isEditMode }, 
        [
          Validators.required,
          Validators.min(1),
          Validators.pattern('^[0-9]+$')
        ]
      ],
      primerNombre: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      segundoNombre: [
        '', 
        [
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      primerApellido: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      segundoApellido: [
        '', 
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$')
        ]
      ],
      telefonoContacto: [
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
      fechaNacimiento: [
        '', 
        [
          Validators.required,
          this.validarEdadMinima(18)
        ]
      ],
      valorEstimadoSolicitud: [
        '', 
        [
          Validators.required,
          Validators.min(0.01),
          Validators.pattern('^[0-9]+(\.[0-9]{1,2})?$')
        ]
      ],
      observaciones: [
        '', 
        [Validators.maxLength(500)]
      ]
    });
  }

  /**
   * Rellenar el formulario con datos del asegurado (modo edición)
   */
  private patchFormValues(): void {
    if (!this.asegurado) return;

    this.aseguradoForm.patchValue({
      numeroIdentificacion: this.asegurado.numeroIdentificacion,
      primerNombre: this.asegurado.primerNombre,
      segundoNombre: this.asegurado.segundoNombre || '',
      primerApellido: this.asegurado.primerApellido,
      segundoApellido: this.asegurado.segundoApellido,
      telefonoContacto: this.asegurado.telefonoContacto,
      email: this.asegurado.email,
      fechaNacimiento: new Date(this.asegurado.fechaNacimiento),
      valorEstimadoSolicitud: this.asegurado.valorEstimadoSolicitud,
      observaciones: this.asegurado.observaciones || ''
    });
  }

  /**
   * Validador personalizado: edad mínima
   */
  private validarEdadMinima(edadMinima: number) {
    return (control: any) => {
      if (!control.value) return null;

      const fechaNacimiento = new Date(control.value);
      const hoy = new Date();
      let edad = hoy.getFullYear() - fechaNacimiento.getFullYear();
      const mes = hoy.getMonth() - fechaNacimiento.getMonth();
      
      if (mes < 0 || (mes === 0 && hoy.getDate() < fechaNacimiento.getDate())) {
        edad--;
      }

      return edad >= edadMinima ? null : { edadMinima: { required: edadMinima, actual: edad } };
    };
  }

  /**
   * Obtener mensaje de error para un campo
   */
  getErrorMessage(fieldName: string): string {
    const control = this.aseguradoForm.get(fieldName);
    if (!control || !control.errors) return '';

    if (control.hasError('required')) return 'Este campo es requerido';
    if (control.hasError('email')) return 'Email inválido';
    if (control.hasError('min')) return `Valor mínimo: ${control.errors['min'].min}`;
    if (control.hasError('minlength')) return `Mínimo ${control.errors['minlength'].requiredLength} caracteres`;
    if (control.hasError('maxlength')) return `Máximo ${control.errors['maxlength'].requiredLength} caracteres`;
    if (control.hasError('pattern')) return 'Formato inválido';
    if (control.hasError('edadMinima')) return `Edad mínima: ${control.errors['edadMinima'].required} años`;

    return 'Campo inválido';
  }

  /**
   * Enviar formulario
   */
  onSubmit(): void {
    if (this.aseguradoForm.invalid) {
      this.aseguradoForm.markAllAsTouched();
      return;
    }

    const formValue = this.aseguradoForm.getRawValue();
    
    // Formatear fecha a ISO 8601
    const fechaNacimiento = new Date(formValue.fechaNacimiento);
    fechaNacimiento.setHours(0, 0, 0, 0);
    
    const aseguradoData: CreateAseguradoDto = {
      ...formValue,
      fechaNacimiento: fechaNacimiento.toISOString(),
      numeroIdentificacion: Number(formValue.numeroIdentificacion),
      valorEstimadoSolicitud: Number(formValue.valorEstimadoSolicitud)
    };

    this.submitForm.emit(aseguradoData);
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
    this.aseguradoForm.reset();
  }
}
