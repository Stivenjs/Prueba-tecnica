import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  Insured, 
  InsuredsResponse, 
  CreateInsuredDto, 
  UpdateInsuredDto,
  SearchResponse 
} from '@shared/models/insured.model';

/**
 * Servicio para gestionar las operaciones CRUD de Asegurados
 * Consume la API RESTful del backend
 */
@Injectable({
  providedIn: 'root'
})
export class InsuredsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5231/api/insureds';

  /**
   * Obtener lista de asegurados con paginación
   * @param pageNumber Número de página (por defecto 1)
   * @param pageSize Tamaño de página (por defecto 10)
   * @returns Observable con la respuesta paginada
   */
  getInsureds(pageNumber: number = 1, pageSize: number = 10): Observable<InsuredsResponse> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<InsuredsResponse>(this.apiUrl, { params });
  }

  /**
   * Obtener un asegurado por su número de identificación
   * @param id Número de identificación
   * @returns Observable con el asegurado
   */
  getInsuredById(id: number): Observable<Insured> {
    return this.http.get<Insured>(`${this.apiUrl}/${id}`);
  }

  /**
   * Buscar asegurados por número de identificación (búsqueda parcial)
   * @param identificationNumber Número de identificación a buscar
   * @returns Observable con el resultado de la búsqueda y metadata
   */
  searchByIdentification(identificationNumber: string): Observable<SearchResponse> {
    return this.http.get<SearchResponse>(`${this.apiUrl}/search/${identificationNumber}`);
  }

  /**
   * Crear un nuevo asegurado
   * @param insured Datos del asegurado a crear
   * @returns Observable con el asegurado creado
   */
  createInsured(insured: CreateInsuredDto): Observable<Insured> {
    return this.http.post<Insured>(this.apiUrl, insured);
  }

  /**
   * Actualizar un asegurado existente
   * @param id Número de identificación
   * @param insured Datos actualizados
   * @returns Observable con el asegurado actualizado
   */
  updateInsured(id: number, insured: UpdateInsuredDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, insured);
  }

  /**
   * Eliminar un asegurado
   * @param id Número de identificación
   * @returns Observable con el resultado de la operación
   */
  deleteInsured(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
