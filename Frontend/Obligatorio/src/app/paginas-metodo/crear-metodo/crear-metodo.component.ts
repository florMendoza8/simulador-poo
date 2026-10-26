import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { HttpClient } from '@angular/common/http';
import { ParametroServicio } from '../../servicios/parametro-servicio.service';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { TipoDato } from '../../modelos/tipo-dato';
import { Accesibilidad } from '../../modelos/accesibilidad';
import { Parametro } from '../../modelos/parametro/parametro';
import { Metodo } from '../../modelos/metodo/metodo';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { CommonModule } from '@angular/common';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { VariableLocal } from '../../modelos/variableLocal/variable';
import { VariableLocalService } from '../../servicios/variable-local.service';
import { firstValueFrom } from 'rxjs';


@Component({
  selector: 'app-crear-metodo',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, BotonCancelarComponent, BotonCrearComponent],
  templateUrl: './crear-metodo.component.html',
  styleUrl: './crear-metodo.component.css'
})
export class CrearMetodoComponent implements OnInit {
  formulario: FormGroup;
  tiposDato: TipoDato[] = [];
  parametrosDisponibles: Parametro[] = [];
  metodosDisponibles: Metodo[] = [];
  parametroSeleccionado: FormControl<Parametro | null>;
  metodoSeleccionado: FormControl<Metodo | null>;
  variablesDisponibles: VariableLocal[] = [];
  variableSeleccionada: FormControl<VariableLocal | null>;
  accesibilidades: string[] = Object.values(Accesibilidad);
  errorAlCargarTipos: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private parametroServicio: ParametroServicio,
    private tipoDatoServicio: TipoDatoServicio,
    private variableLocalServicio: VariableLocalService,
    private metodoServicio: MetodoServicio
  ){
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoDato: ['', Validators.required],
      accesibilidad: this.formBuilder.control(null, Validators.required),
      parametros: this.formBuilder.array([]),
      variablesLocales: this.formBuilder.array([]),
      metodosInvocados: this.formBuilder.array([]),
      abstract: [false],
      sealed: [false],
      static: [false],
      virtual: [false],
      override: [false]
    })
    this.parametroSeleccionado = this.formBuilder.control<Parametro | null>(null);
    this.metodoSeleccionado = this.formBuilder.control<Metodo | null>(null);
    this.variableSeleccionada = this.formBuilder.control<VariableLocal | null>(null);
  }

  ngOnInit(): void {
    this.tipoDatoServicio.obtenerTiposDato().subscribe({
      next: (tipos) => (this.tiposDato = tipos),
      error: (e) => {
        console.error('Error al cargar tipos de dato', e);
      }
    });
    this.parametroServicio.obtenerParametros().subscribe({
      next: (listaParametros) => (this.parametrosDisponibles = listaParametros),
      error: () => console.error('Error al cargar parámetros')
    });
    this.variableLocalServicio.obtenerVariables().subscribe({
      next: (listaVariables) => this.variablesDisponibles = listaVariables,
      error: () => console.error('Error al cargar variables')
    });
    this.metodoServicio.obtenerMetodos().subscribe({
      next: (listaMetodos) => (this.metodosDisponibles = listaMetodos),
      error: () => console.error('Error al cargar métodos')
    });
  }

  get parametros(): FormArray {
    return this.formulario.get('parametros') as FormArray;
  }
  get variables(): FormArray {
    return this.formulario.get('variablesLocales') as FormArray;
  }
  get metodosInvocados(): FormArray {
    return this.formulario.get('metodosInvocados') as FormArray;
  }


  agregarParametro(): void {
    const param = this.parametroSeleccionado.value;
    if (param && !this.parametros.value.some((p: Parametro) => p.id === param.id)) {
      this.parametros.push(this.formBuilder.control(param));
      this.parametroSeleccionado.setValue(null);
    }
  }
  eliminarParametro(index: number): void {
    this.parametros.removeAt(index);
  }

  agregarVariable(): void {
    const variableLocal = this.variableSeleccionada.value;
    if (variableLocal && !this.variables.value.some((v: VariableLocal) => v.id === variableLocal.id)) {
      this.variables.push(this.formBuilder.control(variableLocal));
      this.variableSeleccionada.setValue(null);
    }
  }
  eliminarVariable(index: number): void {
    this.variables.removeAt(index);
  }

  agregarMetodo(): void {
    const metodo = this.metodoSeleccionado.value;
    if (metodo && !this.metodosInvocados.value.some((m: Metodo) => m.id === metodo.id)) {
      this.metodosInvocados.push(this.formBuilder.control(metodo));
      this.metodoSeleccionado.setValue(null);
    }
  }
  eliminarMetodo(index: number): void {
    this.metodosInvocados.removeAt(index);
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

    return `${accesibilidad} ${metodo.nombre}(${firmaParametros})${modStr} : ${tipoRetorno}`;
  }

  rollbackMetodo(metodoId: string, mensaje: string, codigo: number): void {
    this.metodoServicio.eliminarMetodo(metodoId).subscribe({
      next: () => alert(`Error ${codigo}: ${mensaje}`),
      error: () => alert(`Error ${codigo}: ${mensaje}. No se pudo eliminar el método.`)
    });
  }

  crear(): void {
    if (this.formulario.valid) {
      const valor = this.formulario.value;

      const parametrosIds: string[] = valor.parametros.map((p: Parametro) => p.id);
      const variablesLocalesIds: string[] = valor.variablesLocales.map((v: VariableLocal) => v.id);
      const metodosInvocadosIds: string[] = valor.metodosInvocados.map((m: Metodo) => m.id);

      const metodoSolicitud = {
        nombre: valor.nombre,
        tipoRetornoId: valor.tipoDato,
        accesibilidad: valor.accesibilidad,
        esAbstracto: valor.abstract,
        esSealed: valor.sealed,
        esEstatico: valor.static,
        esVirtual: valor.virtual,
        esOverride: valor.override
      };

      this.metodoServicio.crearMetodo(metodoSolicitud).subscribe({
        next: async (nuevoMetodo) => {
          try {
            const tareas: Promise<any>[] = [];

            for (const parametroId of parametrosIds) {
              const tarea = firstValueFrom(this.metodoServicio.agregarParametro(nuevoMetodo.id, parametroId));
              tareas.push(tarea);
            }

            for (const variableLocalId of variablesLocalesIds) {
              const tarea = firstValueFrom(this.metodoServicio.agregarVariableLocal(nuevoMetodo.id, variableLocalId));
              tareas.push(tarea);
            }

            for (const metodoInvocadoId of metodosInvocadosIds) {
              const tarea = firstValueFrom(this.metodoServicio.agregarMetodoInvocado(nuevoMetodo.id, metodoInvocadoId));
              tareas.push(tarea);
            }

            await Promise.all(tareas);

            alert('Método creado correctamente.');
            this.formulario.reset();
            this.formulario.patchValue({
              abstract: false,
              sealed: false,
              static: false,
              virtual: false,
              override: false,
              accesibilidad: null,
              tipoDato: '',
            });
            this.parametros.clear();
            this.metodosInvocados.clear();
            this.variables.clear();
            this.parametroSeleccionado.setValue(null);
            this.variableSeleccionada.setValue(null);
            this.metodoSeleccionado.setValue(null);
          } 
          catch (error: any) {
            const mensaje = error.error?.mensaje || 'Error al agregar elementos.';
            const codigo = error.status;
            this.rollbackMetodo(nuevoMetodo.id, mensaje, codigo);
          }
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al crear método.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      })
    }
  }
}
