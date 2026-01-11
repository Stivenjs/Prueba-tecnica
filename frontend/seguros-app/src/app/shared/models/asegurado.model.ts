/**
 * Modelo de datos para Asegurado
 * Representa la estructura de un asegurado en el sistema
 */
export interface Asegurado {
  numeroIdentificacion: number;
  primerNombre: string;
  segundoNombre?: string;
  primerApellido: string;
  segundoApellido: string;
  telefonoContacto: string;
  email: string;
  fechaNacimiento: string; // ISO 8601 format
  valorEstimadoSolicitud: number;
  observaciones?: string;
  fechaCreacion?: string;
  fechaActualizacion?: string | null;
}

/**
 * Respuesta paginada de la API
 */
export interface AseguradosResponse {
  totalRecords: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  data: Asegurado[];
}

/**
 * DTO para crear un nuevo asegurado
 * (sin campos de auditoría)
 */
export interface CreateAseguradoDto {
  numeroIdentificacion: number;
  primerNombre: string;
  segundoNombre?: string;
  primerApellido: string;
  segundoApellido: string;
  telefonoContacto: string;
  email: string;
  fechaNacimiento: string;
  valorEstimadoSolicitud: number;
  observaciones?: string;
}

/**
 * DTO para actualizar un asegurado existente
 */
export interface UpdateAseguradoDto extends CreateAseguradoDto {}
