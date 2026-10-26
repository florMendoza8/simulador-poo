import { Component, Input } from '@angular/core';
import { BotonEliminarComponent } from '../../botones/boton-eliminar/boton-eliminar.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { FirmaMetodo } from '../../modelos/metodo/firma-metodo';
import { ActivatedRoute, Router } from '@angular/router';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';

@Component({
  selector: 'app-vista-firma-metodo',
  imports: [BotonEliminarComponent, BotonVolverComponent],
  templateUrl: './vista-firma-metodo.component.html',
  styleUrl: './vista-firma-metodo.component.css'
})
export class VistaFirmaMetodoComponent {
  @Input() firmaMetodo!: FirmaMetodo;
  @Input() mostrarVolver: boolean = false;
  @Input() onEliminar?: (id: string) => void;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private metodoServicio: MetodoServicio
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.metodoServicio.obtenerFirmaMetodoPorId(id).subscribe({
        next: (firmaMetodo) => this.firmaMetodo = firmaMetodo,
        error: (error) => console.error('Error al obtener firma de método:', error)
      });

      this.onEliminar = (id: string) => {
      this.metodoServicio.eliminarMetodo(id).subscribe({
        next: () => {
        alert('Firma de método eliminada correctamente.');
        this.router.navigate(['/firma-metodos']);
      },
        error: (e) => console.error('Error al eliminar:', e)
      });
    };
    }
  }
}
