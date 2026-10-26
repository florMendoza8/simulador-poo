import { Accesibilidad } from "../accesibilidad";

export interface ModificarAtributoSolicitud {
    nombre: string;
    tipoReferenciaId: string;
    tipoInstanciaId: string;
    accesibilidad: Accesibilidad;
    esEstatico: boolean;
}
