import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AgregarFirmaMetodoRespuesta } from '../modelos/interfaz/agregar-firma-metodo-respuesta';
import { CrearInterfazSolicitud } from '../modelos/interfaz/crear-interfaz-solicitud';
import { CrearInterfazRespuesta } from '../modelos/interfaz/crear-interfaz-respuesta';
import { environment } from '../../entornos/entorno';
import { Interfaz } from '../modelos/interfaz/interfaz';

@Injectable({
  providedIn: 'root'
})
export class InterfazServicio {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  crearInterfaz(interfaz: CrearInterfazSolicitud): Observable<CrearInterfazRespuesta> {
    return this.http.post<CrearInterfazRespuesta>(`${environment.apiUrl}/interfaces`, interfaz);
  }

  eliminarInterfaz(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/interfaces/${id}`);
  }

  agregarFirmaMetodo(interfazId: string, firmaMetodoId: string): Observable<AgregarFirmaMetodoRespuesta> {
    return this.http.post<AgregarFirmaMetodoRespuesta>(`${this.apiUrl}/interfaces/${interfazId}/firmaMetodos`, {
      firmaMetodoId
    });
  }

  obtenerInterfaces(): Observable<Interfaz[]> {
    return this.http.get<Interfaz[]>(`${environment.apiUrl}/interfaces`);
  }

  obtenerInterfazPorId(id: string): Observable<Interfaz> {
    return this.http.get<Interfaz>(`${this.apiUrl}/interfaces/${id}`);
  }

}
