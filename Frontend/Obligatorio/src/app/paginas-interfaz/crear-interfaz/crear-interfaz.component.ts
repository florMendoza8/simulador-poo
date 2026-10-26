import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { HttpClient } from '@angular/common/http';
import { InterfazServicio } from '../../servicios/interfaz-servicio.service';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { CommonModule } from '@angular/common';
import { FirmaMetodo } from '../../modelos/metodo/firma-metodo';

@Component({
  selector: 'app-crear-interfaz',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, BotonCancelarComponent, BotonCrearComponent],
  templateUrl: './crear-interfaz.component.html',
  styleUrl: './crear-interfaz.component.css'
})
export class CrearInterfazComponent {
  formulario: FormGroup;
  errorAlCargarTipos: boolean = false;
  metodosDisponibles: FirmaMetodo[] = [];
  metodoSeleccionado: FormControl<FirmaMetodo | null>;

 constructor(
    private formBuilder: FormBuilder, 
    private http: HttpClient,
    private interfazServicio: InterfazServicio,
    private metodoServicio: MetodoServicio
  ){
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      firmaMetodos: this.formBuilder.array([])
    })
    this.metodoSeleccionado = this.formBuilder.control<FirmaMetodo | null>(null);
  }

  ngOnInit(): void {
    this.metodoServicio.obtenerFirmaMetodos().subscribe({
    next: (listaMetodos) => (this.metodosDisponibles = listaMetodos),
    error: () => console.error('Error al cargar métodos')
    });
  }

  get firmaMetodos(): FormArray {
    return this.formulario.get('firmaMetodos') as FormArray;
  }

  agregarMetodo(): void {
    const metodo = this.metodoSeleccionado.value;
    if (metodo && !this.firmaMetodos.value.some((m: FirmaMetodo) => m.id === metodo.id)) {
      this.firmaMetodos.push(this.formBuilder.control(metodo));
      this.metodoSeleccionado.setValue(null);
    }
  }
  eliminarMetodo(index: number): void {
    this.firmaMetodos.removeAt(index);
  }

crear(): void {
    if (this.formulario.valid) {
      const valor = this.formulario.value;
      const firmaMetodosIds: string[] = valor.firmaMetodos.map((m: FirmaMetodo) => m.id);

      const interfazSolicitud = {
        nombre: valor.nombre,
      };
      console.log("Solicitud enviada:", interfazSolicitud);

      this.interfazServicio.crearInterfaz(interfazSolicitud).subscribe({
        next: (nuevaInterfaz) => {
          firmaMetodosIds.forEach((firmaMetodoId: string) => {
            this.interfazServicio.agregarFirmaMetodo(nuevaInterfaz.id, firmaMetodoId).subscribe({
              error: (error) => console.error('Error al agregar metodo:', error)
            });
          });

          alert('Interfaz creada correctamente.');
          this.formulario.reset();
          this.firmaMetodos.clear();
          this.metodoSeleccionado.setValue(null);
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al crear interfaz.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
    }
  }
}
