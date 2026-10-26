import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Parametro } from '../modelos/parametro/parametro';
import { environment } from '../../entornos/entorno';
import { CrearParametroRespuesta } from '../modelos/parametro/crear-parametro-respuesta';

@Injectable({
  providedIn: 'root'
})
export class ParametroServicio {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  crearParametro(parametro: { nombre: string; tipoDatoId: string }): Observable<CrearParametroRespuesta> {
    return this.http.post<CrearParametroRespuesta>(`${environment.apiUrl}/parametros`, parametro);
  }

  obtenerParametros(): Observable<Parametro[]> {
    return this.http.get<Parametro[]>(`${environment.apiUrl}/parametros`);
  }

  obtenerParametroPorId(id: string): Observable<Parametro> {
      return this.http.get<Parametro>(`${this.apiUrl}/parametros/${id}`);
  }
}
