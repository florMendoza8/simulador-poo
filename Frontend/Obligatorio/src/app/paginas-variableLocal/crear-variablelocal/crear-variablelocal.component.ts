import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { VariableLocalService } from '../../servicios/variable-local.service';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { TipoDato } from '../../modelos/tipo-dato';
import {VariableLocal} from '../../modelos/variableLocal/variable';
import { Crearvariablelocalrespuesta } from '../../modelos/variableLocal/crearvariablelocalrespuesta';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { CommonModule } from '@angular/common';
import { BotonCrearComponent } from '../../botones/boton-crear/boton-crear.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-crear-variablelocal',
  imports: [ReactiveFormsModule, BotonCancelarComponent, CommonModule, BotonCrearComponent],
  templateUrl: './crear-variablelocal.component.html',
  styleUrl: './crear-variablelocal.component.css'
})
export class CrearVariablelocalComponent implements OnInit {
    formulario: FormGroup;
    tiposDato: TipoDato[] = [];
    error = '';

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private variableService: VariableLocalService,
    private tipoDatoService: TipoDatoServicio
  ) {
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoReferenciaId: ['', Validators.required],
      valor: ['']
    });
  }

   ngOnInit(): void {
     this.tipoDatoService.obtenerTiposDato().subscribe({
      next: (tipos) => (this.tiposDato = tipos),
        error: (e) => {
        console.error('Error al cargar tipos de dato', e);}
    });
  }

    crear(): void {
      console.log('Se ejecutó el método crear()')
    if (this.formulario.valid) {
      const variable: VariableLocal = this.formulario.value;
        console.log('Enviando variable:', variable);
      this.variableService.crear(variable).subscribe({
        next: (res: Crearvariablelocalrespuesta) => {
          alert(`Variable creada con éxito`);
          this.formulario.reset();
          this.formulario.patchValue({
            tipoDato: ''
          });
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al crear variable.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
    }
  }

}
