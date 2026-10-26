import { Component } from '@angular/core';
import { BotonCancelarComponent } from '../../../botones/boton-cancelar/boton-cancelar.component';
import { BotonGuardarCambiosComponent } from '../../../botones/boton-guardar-cambios/boton-guardar-cambios.component';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Clase } from '../../../modelos/clase/clase';
import { Metodo } from '../../../modelos/metodo/metodo';
import { Atributo } from '../../../modelos/atributo/atributo';
import { ClaseServicioService } from '../../../servicios/clase-servicio.service';
import { MetodoServicio } from '../../../servicios/metodo-servicio.service';
import { AtributoServicio } from '../../../servicios/atributo-servicio.service';
import { ModificarClaseSolicitud } from '../../../modelos/clase/modificar-clase-solicitud';

@Component({
  selector: 'app-modificar-clase',
  imports: [ ReactiveFormsModule,CommonModule,RouterModule,BotonCancelarComponent, BotonGuardarCambiosComponent],
  templateUrl: './modificar-clase.component.html',
  styleUrl: './modificar-clase.component.css'
})
export class ModificarClaseComponent {
  formulario!: FormGroup;
  claseId!: string;
  clasesDisponibles: Clase[] = [];
  metodosDisponibles: Metodo[] = [];
  atributosDisponibles: Atributo[] = [];
  metodoSeleccionado: FormControl<Metodo | null>;
  atributoSeleccionado: FormControl<Atributo | null>;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private claseServicio: ClaseServicioService,
    private metodoServicio: MetodoServicio,
    private atributoServicio: AtributoServicio
  ) {
    this.formulario = this.fb.group({
      nombre:    ['', Validators.required],
      esAbstract:[false],
      esSealed:  [false],
      claseBaseId:[''],
      metodos:   this.fb.array<FormControl<Metodo | null>>([]),
      atributos: this.fb.array<FormControl<Atributo | null>>([])
    });

    this.metodoSeleccionado = this.fb.control<Metodo | null>(null);
    this.atributoSeleccionado = this.fb.control<Atributo | null>(null);
  }


claseOriginal!: Clase; // nueva propiedad

ngOnInit(): void {
  this.claseId = this.route.snapshot.paramMap.get('id')!;
   this.claseServicio.obtenerTodas().subscribe(lista => this.clasesDisponibles = lista);
  this.metodoServicio.obtenerMetodos().subscribe(m => this.metodosDisponibles = m);
  this.atributoServicio.obtenerAtributos().subscribe(a => this.atributosDisponibles = a);
  this.claseServicio.obtenerPorId(this.claseId).subscribe({
    next: c => {
      this.claseOriginal = c; 
      this.formulario.patchValue({  
          nombre: c.nombre,
          esAbstract: c.esAbstract,
          esSealed: c.esSealed,
          claseBaseId: c.claseBaseId ?? ''
        });
      c.metodos.forEach(m => this.metodos.push(this.fb.control(m)));
      c.atributos.forEach(a => this.atributos.push(this.fb.control(a)));
    },
    error: (err) => {
      const mensaje = err.error?.mensaje || 'No se pudo cargar la clase.';
      const codigo = err.status;
      alert(`Error ${codigo}: ${mensaje}`);
    }
  });
}

  get metodos(): FormArray { return this.formulario.get('metodos') as FormArray; }
  get atributos(): FormArray { return this.formulario.get('atributos') as FormArray; }

  agregarMetodo() {
    const sel = this.metodoSeleccionado.value;
    if (sel && !this.metodos.value.some((m: Metodo) => m.id === sel.id)) {
      this.metodos.push(this.fb.control(sel));
      this.metodoSeleccionado.reset();
    }
  }
  eliminarMetodo(i: number) { this.metodos.removeAt(i); }

  agregarAtributo() {
    const sel = this.atributoSeleccionado.value;
    if (sel && !this.atributos.value.some((a: Atributo) => a.id === sel.id)) {
      this.atributos.push(this.fb.control(sel));
      this.atributoSeleccionado.reset();
    }
  }
  eliminarAtributo(i: number) { this.atributos.removeAt(i); }

  modificar(): void {
    if (this.formulario.invalid) return;
    const v = this.formulario.value;

    const solicitud: ModificarClaseSolicitud = {
      nombre:    v.nombre,
      esAbstract:v.esAbstract,
      esSealed:  v.esSealed,
      claseBaseId: v.claseBaseId === '' ? null : v.claseBaseId,
    };

    const nuevosMetodos = this.metodos.value as Metodo[];
    const nuevosAtributos = this.atributos.value as Atributo[];

    const metodosOriginales = this.claseOriginal.metodos;
    const atributosOriginales = this.claseOriginal.atributos;

    const metodosAgregados = nuevosMetodos.filter(n => !metodosOriginales.some(o => o.id === n.id));
    const metodosEliminados = metodosOriginales.filter(o => !nuevosMetodos.some(n => n.id === o.id));

    const atributosAgregados = nuevosAtributos.filter(n => !atributosOriginales.some(o => o.id === n.id));
    const atributosEliminados = atributosOriginales.filter(o => !nuevosAtributos.some(n => n.id === o.id));

    this.claseServicio.modificarClase(this.claseId, solicitud).subscribe({
      next: () => {
        const observables = [];

        for (const metodo of metodosAgregados)
          observables.push(this.claseServicio.agregarMetodo(this.claseId, { MetodoId: metodo.id }));
        for (const metodo of metodosEliminados)
          observables.push(this.claseServicio.eliminarMetodo(this.claseId, metodo.id));

        for (const atributo of atributosAgregados)
          observables.push(this.claseServicio.agregarAtributo(this.claseId, { AtributoId: atributo.id }));
        for (const atributo of atributosEliminados)
          observables.push(this.claseServicio.eliminarAtributo(this.claseId, atributo.id));

        Promise.all(observables.map(obs => obs.toPromise()))
          .then(() => {
            alert('Clase modificada correctamente.');
            this.router.navigate(['/clase']);
          })
          .catch((error) => {
            const mensaje = error?.error?.mensaje || 'Error al modificar métodos o atributos.';
            const codigo = error?.status || '???';
            alert(`Error ${codigo}: ${mensaje}`);
          });
      },
      error: (err) => {
        const mensaje = err.error?.mensaje || 'Error al modificar la clase.';
        const codigo = err.status;
        alert(`Error ${codigo}: ${mensaje}`);
      }
    });
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
}
