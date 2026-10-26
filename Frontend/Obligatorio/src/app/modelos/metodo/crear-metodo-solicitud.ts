import { Accesibilidad } from "../accesibilidad";

export interface CrearMetodoSolicitud {
  nombre: string;
  tipoRetornoId: string;
  accesibilidad: Accesibilidad;
  esAbstracto: boolean;
  esSealed: boolean;
  esEstatico: boolean;
  esVirtual: boolean;
  esOverride: boolean;
}

