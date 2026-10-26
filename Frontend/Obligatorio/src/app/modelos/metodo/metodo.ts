import { Accesibilidad } from "../accesibilidad";

export interface Metodo {
    id: string;
    nombre: string;
    tipoRetorno: string | null;
    esAbstract: boolean;
    esSealed: boolean;
    esEstatico: boolean;
    esVirtual: boolean;
    esOverride: boolean;
    accesibilidad: Accesibilidad;
    parametros: { id: string; nombre: string }[];
    variablesLocales: { id: string; nombre: string }[];
    metodosInvocados: { id: string; nombre: string }[];
}
