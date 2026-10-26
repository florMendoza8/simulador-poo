import { Component, Input } from '@angular/core';
import { BotonEliminarComponent } from '../../botones/boton-eliminar/boton-eliminar.component';
import { AtributoServicio } from '../../servicios/atributo-servicio.service';
import { Atributo } from '../../modelos/atributo/atributo';
import { ActivatedRoute, Router } from '@angular/router';
import { BotonModificarComponent } from '../../botones/boton-modificar/boton-modificar.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-atributo',
  imports: [BotonEliminarComponent, BotonModificarComponent, BotonVolverComponent],
  templateUrl: './vista-atributo.component.html',
  styleUrl: './vista-atributo.component.css'
})
export class VistaAtributoComponent {
  @Input() atributo!: Atributo;
  @Input() mostrarVolver: boolean = false;
  @Input() onEliminar?: (id: string) => void;
  @Input() onModificar?: (id: string) => void;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private atributoServicio: AtributoServicio
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.atributoServicio.obtenerAtributoPorId(id).subscribe({
        next: (atributo) => this.atributo = atributo,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener atributo.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });

      this.onEliminar = (id: string) => {
        this.atributoServicio.eliminarAtributo(id).subscribe({
          next: () => {
          alert('Atributo eliminado correctamente.');
          this.router.navigate(['/atributos']);
        },
          error: (error) => {
            const mensaje = error.error?.mensaje || 'Error al eliminar.';
            const codigo = error.status;
            alert(`Error ${codigo}: ${mensaje}`);
          }
        });
      };

      this.onModificar = (id: string) => {
        this.router.navigate([`/atributos/modificar/${id}`]);
      }
    }
  }  
}
