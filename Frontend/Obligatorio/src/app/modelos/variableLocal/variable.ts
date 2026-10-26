import { TipoDato } from "../tipo-dato";

export interface VariableLocal {
    id?: string;
    nombre: string;
    tipoReferenciaId: string | null;
    tipoReferencia?: TipoDato;
    valor: string;
}
