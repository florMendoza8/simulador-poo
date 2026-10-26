import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Atributo } from '../modelos/atributo/atributo';
import { environment } from '../../entornos/entorno';
import { CrearAtributoRespuesta } from '../modelos/atributo/crear-atributo-respuesta';
import { CrearAtributoSolicitud } from '../modelos/atributo/crear-atributo-solicitud';
import { ModificarAtributoSolicitud } from '../modelos/atributo/modificar-atributo-solicitud';

@Injectable({
  providedIn: 'root'
})
export class AtributoServicio {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  crearAtributo(atributo: CrearAtributoSolicitud): Observable<CrearAtributoRespuesta> {
    return this.http.post<CrearAtributoRespuesta>(`${environment.apiUrl}/atributos`, atributo);
  }

  modificarAtributo(id: string, solicitud: ModificarAtributoSolicitud): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/atributos/${id}`, solicitud);
  }

  eliminarAtributo(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/atributos/${id}`);
  }

  obtenerAtributoPorId(id: string): Observable<Atributo> {
    return this.http.get<Atributo>(`${this.apiUrl}/atributos/${id}`);
  }

  obtenerAtributos(): Observable<Atributo[]> {
    return this.http.get<Atributo[]>(`${environment.apiUrl}/atributos`);
  }
}
