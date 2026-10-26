import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AtributoServicio } from '../../servicios/atributo-servicio.service';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { TipoDato } from '../../modelos/tipo-dato';
import { Accesibilidad } from '../../modelos/accesibilidad';
import { ModificarAtributoSolicitud } from '../../modelos/atributo/modificar-atributo-solicitud';
import { BotonCancelarComponent } from '../../botones/boton-cancelar/boton-cancelar.component';
import { CommonModule } from '@angular/common';
import { BotonGuardarCambiosComponent } from '../../botones/boton-guardar-cambios/boton-guardar-cambios.component';
import { BotonEliminarComponent } from '../../botones/boton-eliminar/boton-eliminar.component';


@Component({
  selector: 'app-modificar-atributo',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, BotonCancelarComponent, BotonGuardarCambiosComponent, BotonEliminarComponent],
  templateUrl: './modificar-atributo.component.html',
  styleUrl: './modificar-atributo.component.css'
})
export class ModificarAtributoComponent {
  formulario: FormGroup;
  tiposDato: TipoDato[] = [];
  accesibilidades = Object.values(Accesibilidad);
  atributoId!: string;
  @Input() onCancelar?: (id: string) => void;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private atributoServicio: AtributoServicio,
    private tipoDatoServicio: TipoDatoServicio,
    private router: Router
  ) {
    this.formulario = this.formBuilder.group({
      nombre: ['', Validators.required],
      tipoDato: ['', Validators.required],
      accesibilidad: this.formBuilder.control(null, Validators.required),
      esEstatico: [false]
    })
  }

  ngOnInit(): void {
    this.atributoId = this.route.snapshot.paramMap.get('id')!;
    this.tipoDatoServicio.obtenerTiposDato().subscribe({
        next: (tipos) => this.tiposDato = tipos,
        error: () => console.error('Error al cargar tipos de dato')
    });

    this.atributoServicio.obtenerAtributoPorId(this.atributoId).subscribe({
      next: (atributo) => {
        this.formulario.patchValue({
          nombre: atributo.nombre,
          tipoDato: atributo.tipoReferencia.id,
          accesibilidad: atributo.accesibilidad,
          esEstatico: atributo.esEstatico
        });
      },
      error: () => console.error('Error al obtener el atributo')
    });
  }

  modificar(): void {
    if (this.formulario.valid) {
      const valor = this.formulario.value;

      const solicitud: ModificarAtributoSolicitud = {
        nombre: valor.nombre,
        tipoReferenciaId: valor.tipoDato,
        tipoInstanciaId: valor.tipoDato,
        accesibilidad: valor.accesibilidad,
        esEstatico: valor.esEstatico
      };

       console.log('Solicitud a enviar:', solicitud);

      this.atributoServicio.modificarAtributo(this.atributoId, solicitud).subscribe({
        next: () => {
          alert('Atributo modificado correctamente.');
          this.router.navigate(['/atributos', this.atributoId]);
        },
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al modificar atributo.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
    }
  }
}
