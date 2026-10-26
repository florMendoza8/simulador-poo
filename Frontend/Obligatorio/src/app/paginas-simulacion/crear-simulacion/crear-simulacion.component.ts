import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonSimularComponent } from '../../botones/boton-simular/boton-simular.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { CommonModule } from '@angular/common';
import { Metodo } from '../../modelos/metodo/metodo';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { SimulacionService } from '../../servicios/simulacion.service';
import { ClaseServicioService } from '../../servicios/clase-servicio.service';

@Component({
  selector: 'app-crear-simulacion',
  imports: [ CommonModule, FormsModule, ReactiveFormsModule, BotonSimularComponent, BotonVolverComponent],
  templateUrl: './crear-simulacion.component.html',
  styleUrl: './crear-simulacion.component.css'
})
export class CrearSimulacionComponent implements OnInit {
  formulario: FormGroup;
  clases: { id: string; nombre: string }[] = [];
  metodos: { id: string; nombre: string }[] = [];
  transformadores: string[] = [];
  metodoSeleccionado: FormControl<Metodo | null>;
  resultado: string | null = null;
  mensajeError: string = '';

  constructor (
    private formBuilder: FormBuilder,
    private metodoServicio: MetodoServicio,
    private claseServicio: ClaseServicioService,
    private simulacionServicio: SimulacionService
  ){
    this.formulario = this.formBuilder.group({
      clave: [''],
      tipoReferenciaId: ['', Validators.required],
      tipoObjetoId: ['', Validators.required],
      metodoReferenciaId: ['', Validators.required],
      transformador: ['']
    })
    this.metodoSeleccionado = this.formBuilder.control<Metodo | null>(null);
  }

  ngOnInit(): void {
    this.claseServicio.obtenerTodas().subscribe(data => this.clases = data);
    this.metodoServicio.obtenerMetodos().subscribe(data => this.metodos = data);
    this.simulacionServicio.obtenerTransformadores().subscribe( data => 
      this.transformadores = data
    );
  }

  simular(): void {
    if (this.formulario.valid) {
      const solicitud = this.formulario.value;
      this.simulacionServicio.ejecutarSimulacion(solicitud).subscribe({
        next: respuesta => {
          const sinTransformador = !solicitud.transformador?.trim();
          if(sinTransformador){
            this.resultado = respuesta.resultado;
          } 
          else{
            this.resultado = respuesta.resultado;
          }
        },
        error: err => {
          const mensaje = err?.error?.mensaje ?? 'Error desconocido';
          const status = err?.status ?? 'Sin código';
          alert(`Error ${status}: ${mensaje}`);
        }
      });

    }
  }

  limpiar(): void {
    this.formulario.reset({
      clave: '',
      tipoReferenciaId: '',
      tipoObjetoId: '',
      metodoReferenciaId: '',
      transformador: ''
    });
    this.resultado = null;
  }
}
