import { Component, Input } from '@angular/core';
import { BotonEliminarComponent } from '../../botones/boton-eliminar/boton-eliminar.component';
import { Interfaz } from '../../modelos/interfaz/interfaz';
import { InterfazServicio } from '../../servicios/interfaz-servicio.service';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-vista-interfaz',
  imports: [BotonEliminarComponent, BotonVolverComponent],
  templateUrl: './vista-interfaz.component.html',
  styleUrl: './vista-interfaz.component.css'
})
export class VistaInterfazComponent {
  @Input() interfaz!: Interfaz;
  @Input() mostrarVolver: boolean = false;
  @Input() onEliminar?: (id: string) => void;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private interfazServicio : InterfazServicio
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.interfazServicio.obtenerInterfazPorId(id).subscribe({
        next: (interfaz) => this.interfaz = interfaz,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener interfaz.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });

      this.onEliminar = (id: string) => {
      this.interfazServicio.eliminarInterfaz(id).subscribe({
        next: () => {
        alert('Interfaz eliminada correctamente.');
        this.router.navigate(['/interfaces']);
      },
          error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al eliminar.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
    };
    }
  }
}
