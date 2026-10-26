import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VariableLocal } from '../modelos/variableLocal/variable';
import { Observable } from 'rxjs';
import { Crearvariablelocalrespuesta } from '../modelos/variableLocal/crearvariablelocalrespuesta'
import { environment } from '../../entornos/entorno';
import { CrearVariableLocalSolicitud } from '../modelos/variableLocal/crear-variable-local-solicitud';

@Injectable({
  providedIn: 'root'
})
export class VariableLocalService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  crear(variable: CrearVariableLocalSolicitud): Observable<Crearvariablelocalrespuesta> {
  return this.http.post<Crearvariablelocalrespuesta>(`${this.apiUrl}/variableLocal`, {
    nombre: variable.nombre,
    tipoReferenciaId: variable.tipoReferenciaId,
    valor: variable.valor
  });
  }

   obtenerPorId(id: string): Observable<VariableLocal> {
    return this.http.get<VariableLocal>(`${this.apiUrl}/variableLocal/${id}`);
  }

  obtenerVariables(): Observable<VariableLocal[]> {
    return this.http.get<VariableLocal[]>(`${this.apiUrl}/variableLocal`);
  }
}
