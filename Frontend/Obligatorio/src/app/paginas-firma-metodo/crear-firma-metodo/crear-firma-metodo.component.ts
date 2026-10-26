import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { HttpClient } from '@angular/common/http';
import { ParametroServicio } from '../../servicios/parametro-servicio.service';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { TipoDato } from '../../modelos/tipo-dato';
import { Parametro } from '../../modelos/parametro/parametro';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { CommonModule } from '@angular/common';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';

@Component({
  selector: 'app-crear-firma-metodo',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, BotonCancelarComponent, BotonCrearComponent],
  templateUrl: './crear-firma-metodo.component.html',
  styleUrl: './crear-firma-metodo.component.css'
})
export class CrearFirmaMetodoComponent implements OnInit {
  formulario: FormGroup;
  tiposDato: TipoDato[] = [];
  parametrosDisponibles: Parametro[] = [];
  parametroSeleccionado: FormControl<Parametro | null>;
  errorAlCargarTipos: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private parametroServicio: ParametroServicio,
    private tipoDatoServicio: TipoDatoServicio,
    private metodoServicio: MetodoServicio
  ){
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoDato: ['', Validators.required],
      parametros: this.formBuilder.array([]),
    })
    this.parametroSeleccionado = this.formBuilder.control<Parametro | null>(null);
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
  }

  get parametros(): FormArray {
    return this.formulario.get('parametros') as FormArray;
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

  crear(): void {
    if (this.formulario.valid) {
      const valor = this.formulario.value;
      const parametrosIds: string[] = valor.parametros.map((p: Parametro) => p.id);

      const firmaMetodoSolicitud = {
        nombre: valor.nombre,
        tipoRetornoId: valor.tipoDato,
      };

      this.metodoServicio.crearFirmaMetodo(firmaMetodoSolicitud).subscribe({
        next: (nuevoMetodo) => {
          parametrosIds.forEach((parametroId: string) => {
            this.metodoServicio.agregarParametro(nuevoMetodo.id, parametroId).subscribe({
              error: (error) => console.error('Error al agregar parámetro:', error)
            });
          });

          alert('Método creado correctamente.');
          this.formulario.reset();

          this.formulario.patchValue({
            tipoDato: '',
          });
          this.parametros.clear();
          this.parametroSeleccionado.setValue(null);
        },
        error: (error) => {
          console.error('Error del backend:', error);
          alert('Error al crear método.') }
      });
    }
  }
}

