export interface FirmaMetodo {
    id: string;
    nombre: string;
    tipoRetorno: string | null;
    parametros: { id: string; nombre: string }[];
}
