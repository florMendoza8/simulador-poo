import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TipoDato } from '../modelos/tipo-dato';
import { environment } from '../../entornos/entorno';

@Injectable({ 
  providedIn: 'root'
})
export class TipoDatoServicio {
  
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  obtenerTiposDato(): Observable<TipoDato[]> {
    return this.http.get<TipoDato[]>(`${environment.apiUrl}/tipoDatos`);
  }

  obtenerTipoPorId(id: string): Observable<TipoDato> {
      return this.http.get<TipoDato>(`${this.apiUrl}/tipoDatos/${id}`);
  }
}
