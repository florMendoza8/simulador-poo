import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { TipoDato } from '../../modelos/tipo-dato';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { HttpClient } from '@angular/common/http';
import { AtributoServicio } from '../../servicios/atributo-servicio.service';
import { Accesibilidad } from '../../modelos/accesibilidad';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-crear-atributo',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, BotonCancelarComponent, BotonCrearComponent],
  templateUrl: './crear-atributo.component.html',
  styleUrl: './crear-atributo.component.css'
})
export class CrearAtributoComponent {
  formulario: FormGroup;
  tiposDato: TipoDato[] = [];
  accesibilidades: string[] = Object.values(Accesibilidad);
  errorAlCargarTipos: boolean = false;

  constructor(
    private formBuilder: FormBuilder, 
    private http: HttpClient,
    private atributoServicio: AtributoServicio,
    private tipoDatoServicio: TipoDatoServicio
  ){
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoDato: ['', Validators.required],
      accesibilidad: this.formBuilder.control(null, Validators.required),
      esEstatico: [false]
    })
  }

  ngOnInit(): void {
    this.tipoDatoServicio.obtenerTiposDato().subscribe({
      next: (tipos) => this.tiposDato = tipos,
      error: () => {
        this.errorAlCargarTipos = true;
        console.error('Error al cargar tipos de dato');
      }
    });
  }

  crear(): void {
    if (this.formulario.valid) {
      const valor = this.formulario.value;

      const atributoSolicitud = {
        nombre: valor.nombre,
        tipoReferenciaId: valor.tipoDato,
        tipoInstanciaId: valor.tipoDato,
        accesibilidad: valor.accesibilidad,
        esEstatico: valor.esEstatico
      }

      this.atributoServicio.crearAtributo(atributoSolicitud).subscribe({
        next: () => {
          alert('Atributo creado correctamente.');

          this.formulario.reset();

          this.formulario.patchValue({
            esEstatico: false,
            tipoDato: '',
          });
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al crear atributo.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });

    }
  }

}
