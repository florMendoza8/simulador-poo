import { Accesibilidad } from "../accesibilidad";
import { TipoDato } from "../tipo-dato";

export interface Atributo {
  id: string;
  nombre: string;
  tipoReferencia: TipoDato;
  tipoInstancia: TipoDato;
  accesibilidad: Accesibilidad;
  esEstatico: boolean;
}
