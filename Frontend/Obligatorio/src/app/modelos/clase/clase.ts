
export interface Clase {
    id: string;
    nombre: string;
    esAbstract: boolean;
    esSealed: boolean;
    metodos:  { id: string; nombre: string }[]; 
    atributos:  { id: string; nombre: string }[];
    claseBaseId: string | null;
}
