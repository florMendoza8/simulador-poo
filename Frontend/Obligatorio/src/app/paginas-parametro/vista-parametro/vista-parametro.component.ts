import { Component, Input } from '@angular/core';
import { Parametro } from '../../modelos/parametro/parametro';
import { ActivatedRoute, Router } from '@angular/router';
import { ParametroServicio } from '../../servicios/parametro-servicio.service';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-parametro',
  imports: [ BotonVolverComponent ],
  templateUrl: './vista-parametro.component.html',
  styleUrl: './vista-parametro.component.css'
})
export class VistaParametroComponent {
  @Input() parametro!: Parametro;
  @Input() mostrarVolver: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private parametroServicio: ParametroServicio
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.parametroServicio.obtenerParametroPorId(id).subscribe({
        next: (parametro) => this.parametro = parametro,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener parámetro.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });      
    };
  }
}
