import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Metodo } from '../modelos/metodo/metodo';
import { environment } from '../../entornos/entorno';
import { CrearMetodoSolicitud } from '../modelos/metodo/crear-metodo-solicitud';
import { AgregarParametroMetodoRespuesta } from '../modelos/metodo/agregar-parametro-metodo-respuesta';
import { AgregarMetodoInvocadoMetodoRespuesta } from '../modelos/metodo/agregar-metodo-invocado-metodo-respuesta';
import { CrearMetodoRespuesta } from '../modelos/metodo/crear-metodo-respuesta';
import { AgregarVariableLocalMetodoRespuesta } from '../modelos/metodo/agregar-variable-local-metodo-respuesta';
import { CrearFirmaMetodoSolicitud } from '../modelos/metodo/crear-firma-metodo-solicitud';
import { FirmaMetodo } from '../modelos/metodo/firma-metodo';

@Injectable({
  providedIn: 'root'
})
export class MetodoServicio {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  crearMetodo(metodo: CrearMetodoSolicitud): Observable<CrearMetodoRespuesta> {
    return this.http.post<CrearMetodoRespuesta>(`${environment.apiUrl}/metodos`, metodo);
  }

  agregarParametro(metodoId: string, parametroId: string): Observable<AgregarParametroMetodoRespuesta> {
    return this.http.post<AgregarParametroMetodoRespuesta>(`${this.apiUrl}/metodos/${metodoId}/parametros`, {
      parametroId
    });
  }

  agregarVariableLocal(metodoId: string, variableLocalId: string): Observable<AgregarVariableLocalMetodoRespuesta> {
    return this.http.post<AgregarVariableLocalMetodoRespuesta>(`${this.apiUrl}/metodos/${metodoId}/variablesLocales`, {
      variableLocalId
    });
  }

  agregarMetodoInvocado(metodoId: string, metodoInvocadoId: string): Observable<AgregarMetodoInvocadoMetodoRespuesta> {
    return this.http.post<AgregarMetodoInvocadoMetodoRespuesta>(`${this.apiUrl}/metodos/${metodoId}/metodosInvocados`, {
      metodoInvocadoId
    });
  }

  obtenerMetodos(): Observable<Metodo[]> {
    return this.http.get<Metodo[]>(`${environment.apiUrl}/metodos`);
  }

  obtenerMetodoPorId(id: string): Observable<Metodo> {
    return this.http.get<Metodo>(`${this.apiUrl}/metodos/${id}`);
  }

  eliminarMetodo(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/metodos/${id}`);
  }

  crearFirmaMetodo(metodo: CrearFirmaMetodoSolicitud): Observable<CrearMetodoRespuesta> {
    return this.http.post<CrearMetodoRespuesta>(`${environment.apiUrl}/metodos/firmas`, metodo);
  }

  obtenerFirmaMetodos(): Observable<FirmaMetodo[]> {
    return this.http.get<FirmaMetodo[]>(`${environment.apiUrl}/metodos/firmas`);
  }

  obtenerFirmaMetodoPorId(id: string): Observable<FirmaMetodo> {
    return this.http.get<FirmaMetodo>(`${this.apiUrl}/metodos/${id}/firma`);
  }
}
