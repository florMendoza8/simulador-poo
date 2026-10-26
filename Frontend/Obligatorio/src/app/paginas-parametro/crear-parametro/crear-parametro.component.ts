import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { ParametroServicio } from '../../servicios/parametro-servicio.service';
import { TipoDato } from '../../modelos/tipo-dato';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-crear-parametro',
  imports: [CommonModule, ReactiveFormsModule, BotonCancelarComponent, BotonCrearComponent],
  templateUrl: './crear-parametro.component.html',
  styleUrls: ['./crear-parametro.component.css']
})
export class CrearParametroComponent {
  formulario: FormGroup;
  tiposDato: TipoDato[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private parametroServicio: ParametroServicio,
    private tipoDatoServicio: TipoDatoServicio
  ){
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoDato: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.tipoDatoServicio.obtenerTiposDato().subscribe({
      next: (tipos) => this.tiposDato = tipos,
      error: () => {
        console.error('Error al cargar tipos de dato');
      }
    });
  }

  crear(): void {
    if (this.formulario.valid) {
      this.parametroServicio.crearParametro({
        nombre: this.formulario.value.nombre,
        tipoDatoId: this.formulario.value.tipoDato
      }).subscribe({
        next: () => {
          alert('Parámetro creado correctamente.');
          this.formulario.reset();

          this.formulario.patchValue({
            tipoDato: ''
          });
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al crear parámetro.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
    }
  }
}
