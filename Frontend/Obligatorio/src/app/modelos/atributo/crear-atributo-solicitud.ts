import { Accesibilidad } from "../accesibilidad";

export interface CrearAtributoSolicitud {
    nombre: string;
    tipoReferenciaId: string;
    tipoInstanciaId: string;
    accesibilidad: Accesibilidad;
    esEstatico: boolean;
}
