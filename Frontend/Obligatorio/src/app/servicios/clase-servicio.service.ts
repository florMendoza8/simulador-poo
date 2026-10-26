import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../entornos/entorno';
import { CrearClaseSolicitud } from '../modelos/clase/crear-clase-solicitud';
import { CrearClaseRespuesta } from '../modelos/clase/crear-clase-respuesta';
import { Clase } from '../modelos/clase/clase';
import { ModificarClaseSolicitud } from '../modelos/clase/modificar-clase-solicitud';
import { AgregarMetodoSolicitud } from '../modelos/clase/agregar-metodo-solicitud';
import { AgregarMetodoRespuesta } from '../modelos/clase/agregar-metodo-respuesta';
import { AgregarAtributoSolicitud } from '../modelos/clase/agregar-atributo-solicitud';
import { AgregarAtributoRespuesta } from '../modelos/clase/agregar-atributo-respuesta';

@Injectable({
  providedIn: 'root'
})
export class ClaseServicioService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  crearClase(solicitud: CrearClaseSolicitud): Observable<CrearClaseRespuesta> {
    return this.http.post<CrearClaseRespuesta>(`${this.apiUrl}/clases`, solicitud);
  }

  obtenerTodas(): Observable<Clase[]> {
    return this.http.get<Clase[]>(`${this.apiUrl}/clases`);
  }

  obtenerPorId(id: string): Observable<Clase> {
    return this.http.get<Clase>(`${this.apiUrl}/clases/${id}`);
  }

  modificarClase(id: string, solicitud: ModificarClaseSolicitud): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/clases/${id}`, solicitud);
  }

  eliminarClase(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/clases/${id}`);
  }

  agregarMetodo(claseId: string, solicitud: AgregarMetodoSolicitud): Observable<AgregarMetodoRespuesta> {
    return this.http.post<AgregarMetodoRespuesta>(`${this.apiUrl}/clases/${claseId}/metodo`, solicitud);
  }

   agregarAtributo(claseId: string, solicitud: AgregarAtributoSolicitud): Observable<AgregarAtributoRespuesta> {
    return this.http.post<AgregarAtributoRespuesta>(`${this.apiUrl}/clases/${claseId}/atributo`, solicitud);
  }

  eliminarMetodo(claseId: string, metodoId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/clases/${claseId}/metodo/${metodoId}`);
  }

  eliminarAtributo(claseId: string, atributoId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/clases/${claseId}/atributo/${atributoId}`);
  }
}
