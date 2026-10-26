import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ClaseServicioService } from '../../servicios/clase-servicio.service';
import { CrearClaseSolicitud } from '../../modelos/clase/crear-clase-solicitud';
import { Clase } from '../../modelos/clase/clase';
import { Metodo } from '../../modelos/metodo/metodo';
import { Atributo } from '../../modelos/atributo/atributo';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { AtributoServicio } from '../../servicios/atributo-servicio.service';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import {AgregarMetodoSolicitud} from '../../modelos/clase/agregar-metodo-solicitud';
import { AgregarMetodoRespuesta } from '../../modelos/clase/agregar-metodo-respuesta';
import { AgregarAtributoRespuesta } from '../../modelos/clase/agregar-atributo-respuesta';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-crear-clase',
  imports: [ReactiveFormsModule ,BotonCrearComponent, BotonCancelarComponent],
  templateUrl: './crear-clase.component.html',
  styleUrl: './crear-clase.component.css'
})
export class CrearClaseComponent {
   formulario: FormGroup;
   clasesDisponibles: Clase[] = [];
   claseSeleccionada: FormControl<Clase | null>;
   metodosDisponibles: Metodo[] = [];
   atributosDisponibles: Atributo[] = [];

    metodoSeleccionado: FormControl<Metodo | null>;
    atributoSeleccionado: FormControl<Atributo | null>;
   errorAlCargar: boolean = false;
   error: string | null = null;

   constructor(
    private fb: FormBuilder,
    private claseServicio: ClaseServicioService,
    private metodoServicio: MetodoServicio,
    private atributoServicio: AtributoServicio,
    private router: Router
  ) {this.formulario = this.fb.group({
      nombre: ['', Validators.required],
      abstract: [false],
      sealed: [false],
      claseBaseId: [''],
      metodos: this.fb.array([]),
      atributos: this.fb.array([])
    });

    this.claseSeleccionada = this.fb.control<Clase | null>(null);
     this.metodoSeleccionado = this.fb.control<Metodo | null>(null);
    this.atributoSeleccionado = this.fb.control<Atributo | null>(null);
  }

  ngOnInit(): void {
   this.claseServicio.obtenerTodas().subscribe({
      next: (clases) => this.clasesDisponibles = clases,
      error: (e) => {
        console.error('Error al cargar clases disponibles', e);
        this.errorAlCargar = true;
      }
    });

      this.metodoServicio.obtenerMetodos().subscribe({
      next: metodos => this.metodosDisponibles = metodos,
      error: e => console.error('Error al cargar métodos', e)
    });

    
    this.atributoServicio.obtenerAtributos().subscribe({
      next: atributos => this.atributosDisponibles = atributos,
      error: e => console.error('Error al cargar atributos', e)
    });
  }

  get metodos(): FormArray {
    return this.formulario.get('metodos') as FormArray;
  }

  
  get atributos(): FormArray {
    return this.formulario.get('atributos') as FormArray;
  }

  agregarMetodo(): void {
    const metodo = this.metodoSeleccionado.value;
    if (metodo && !this.metodos.value.some((m: Metodo) => m.id === metodo.id)) {
      this.metodos.push(this.fb.control(metodo));
      this.metodoSeleccionado.setValue(null);
    }
  }

  eliminarMetodo(index: number): void {
    this.metodos.removeAt(index);
  }


  agregarAtributo(): void {
    const atributo = this.atributoSeleccionado.value;
    if (atributo && !this.atributos.value.some((a: Atributo) => a.id === atributo.id)) {
      this.atributos.push(this.fb.control(atributo));
      this.atributoSeleccionado.setValue(null);
    }
  }

  eliminarAtributo(index: number): void {
    this.atributos.removeAt(index);
  }

  formatearFirmaMetodo(metodo: Metodo): string {
    const mods = [];
    if (metodo.esAbstract) mods.push("abstract");
    if (metodo.esVirtual) mods.push("virtual");
    if (metodo.esOverride) mods.push("override");
    if (metodo.esEstatico) mods.push("static");
    if (metodo.esSealed) mods.push("sealed");

    const modStr = mods.length ? ` (${mods.join(', ')})` : '';
    const tipoRetorno = metodo.tipoRetorno || 'void';

    const firmaParametros = metodo.parametros?.length
      ? metodo.parametros.map(p => `${p.nombre}`).join(', ')
      : '';

    const accesibilidad = metodo.accesibilidad || 'public';

    return `${metodo.nombre}(${firmaParametros})${modStr} : ${tipoRetorno}`;
  }


  crear(): void {
    if (this.formulario.invalid) return;
    const valor = this.formulario.value;

    const metodoId: string[] = valor.metodos.map((m: Metodo) => m.id);
    const atributoId: string[] = valor.atributos.map((a: Atributo) => a.id);
    const solicitud: CrearClaseSolicitud = {
      nombre: valor.nombre,
      esAbstract: valor.abstract,
      esSealed: valor.sealed,
      claseBaseId: valor.claseBaseId === '' ? null : valor.claseBaseId
    };

    this.claseServicio.crearClase(solicitud).subscribe({
        next: async (respuesta) => {
          const claseId = respuesta.id;
          
          try{
            const tareas: Promise<AgregarMetodoRespuesta | AgregarAtributoRespuesta>[] = [];

            for (const metodo of metodoId) {
              tareas.push(firstValueFrom(this.claseServicio.agregarMetodo(claseId, { MetodoId: metodo })));
            }

            for (const atributo of atributoId) {
              tareas.push(firstValueFrom(this.claseServicio.agregarAtributo(claseId, { AtributoId: atributo })));
            }

            await Promise.all(tareas);
            alert('Clase creada correctamente.');
            this.formulario.reset();
            this.metodos.clear();
            this.atributos.clear()
          }
          catch (error: any) {
            const mensaje = error.error?.mensaje ?? 'Error al agregar elementos a la clase.';
            const codigo = error.status;

            this.claseServicio.eliminarClase(claseId).subscribe({
              next: () => alert(`Error ${codigo}: ${mensaje}`),
              error: () => alert(`Error ${codigo}: ${mensaje}. Además no se pudo eliminar la clase.`)
            });
          }
        },
        error: (e) => {
          const mensaje = e.error?.mensaje ?? 'Error al crear la clase.';
          const codigo = e.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
    });
  }
}
