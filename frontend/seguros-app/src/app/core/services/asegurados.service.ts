import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  Asegurado, 
  AseguradosResponse, 
  CreateAseguradoDto, 
  UpdateAseguradoDto,
  SearchResponse 
} from '@shared/models/asegurado.model';

/**
 * Servicio para gestionar las operaciones CRUD de Asegurados
 * Consume la API RESTful del backend
 */
@Injectable({
  providedIn: 'root'
})
export class AseguradosService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5231/api/asegurados';

  /**
   * Obtener lista de asegurados con paginación
   * @param pageNumber Número de página (por defecto 1)
   * @param pageSize Tamaño de página (por defecto 10)
   * @returns Observable con la respuesta paginada
   */
  getAsegurados(pageNumber: number = 1, pageSize: number = 10): Observable<AseguradosResponse> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<AseguradosResponse>(this.apiUrl, { params });
  }

  /**
   * Obtener un asegurado por su número de identificación
   * @param id Número de identificación
   * @returns Observable con el asegurado
   */
  getAseguradoById(id: number): Observable<Asegurado> {
    return this.http.get<Asegurado>(`${this.apiUrl}/${id}`);
  }

  /**
   * Buscar asegurados por número de identificación (búsqueda parcial)
   * @param numeroIdentificacion Número de identificación a buscar
   * @returns Observable con el resultado de la búsqueda y metadata
   */
  buscarPorIdentificacion(numeroIdentificacion: string): Observable<SearchResponse> {
    return this.http.get<SearchResponse>(`${this.apiUrl}/buscar/${numeroIdentificacion}`);
  }

  /**
   * Crear un nuevo asegurado
   * @param asegurado Datos del asegurado a crear
   * @returns Observable con el asegurado creado
   */
  createAsegurado(asegurado: CreateAseguradoDto): Observable<Asegurado> {
    return this.http.post<Asegurado>(this.apiUrl, asegurado);
  }

  /**
   * Actualizar un asegurado existente
   * @param id Número de identificación
   * @param asegurado Datos actualizados
   * @returns Observable con el asegurado actualizado
   */
  updateAsegurado(id: number, asegurado: UpdateAseguradoDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, asegurado);
  }

  /**
   * Eliminar un asegurado
   * @param id Número de identificación
   * @returns Observable con el resultado de la operación
   */
  deleteAsegurado(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
