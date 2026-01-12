/**
 * Modelo de datos para Asegurado
 * Representa la estructura de un asegurado en el sistema
 */
export interface Insured {
  identificationNumber: number;
  firstName: string;
  middleName?: string;
  firstLastName: string;
  secondLastName: string;
  contactPhone: string;
  email: string;
  birthDate: string; // ISO 8601 format
  estimatedRequestValue: number;
  observations?: string;
  createdAt?: string;
  updatedAt?: string | null;
}

/**
 * Respuesta paginada de la API
 */
export interface InsuredsResponse {
  totalRecords: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  data: Insured[];
}

/**
 * DTO para crear un nuevo asegurado
 * (sin campos de auditoría)
 */
export interface CreateInsuredDto {
  identificationNumber: number;
  firstName: string;
  middleName?: string;
  firstLastName: string;
  secondLastName: string;
  contactPhone: string;
  email: string;
  birthDate: string;
  estimatedRequestValue: number;
  observations?: string;
}

/**
 * DTO para actualizar un asegurado existente
 */
export interface UpdateInsuredDto extends CreateInsuredDto {}

/**
 * Respuesta de búsqueda con metadata
 */
export interface SearchResponse {
  results: Insured[];
  totalCount: number;
  searchTerm?: string;
  message?: string;
}
