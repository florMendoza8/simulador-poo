import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../entornos/entorno';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CrearSimulacionRespuesta } from '../modelos/simulacion/crear-simulacion-respuesta';
import { CrearSimulacionSolicitud } from '../modelos/simulacion/crear-simulacion-solicitud';

@Injectable({
  providedIn: 'root'
})
export class SimulacionService {
  
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  ejecutarSimulacion(simulacion: CrearSimulacionSolicitud & { clave: string }): Observable<CrearSimulacionRespuesta> {
    const headers = new HttpHeaders({
      'X-API-KEY': simulacion.clave
    });

    const { clave, ...body } = simulacion;

    return this.http.post<CrearSimulacionRespuesta>(`${this.apiUrl}/simulaciones`, body, { headers });
  }

  obtenerTransformadores(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/simulaciones/transformadores`);
  }
}
