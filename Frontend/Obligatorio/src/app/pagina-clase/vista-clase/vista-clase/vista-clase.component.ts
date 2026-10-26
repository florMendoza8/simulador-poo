import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BotonVolverComponent } from '../../../botones/boton-volver/boton-volver.component';
import { BotonEliminarComponent } from '../../../botones/boton-eliminar/boton-eliminar.component';
import { Clase } from '../../../modelos/clase/clase';
import { ClaseServicioService } from '../../../servicios/clase-servicio.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BotonModificarComponent } from '../../../botones/boton-modificar/boton-modificar.component';
import { Metodo } from '../../../modelos/metodo/metodo';
import { MetodoServicio } from '../../../servicios/metodo-servicio.service';

@Component({
  selector: 'app-vista-clase',
  standalone: true,
  imports: [CommonModule,BotonVolverComponent, BotonEliminarComponent, BotonModificarComponent],
  templateUrl: './vista-clase.component.html',
  styleUrl: './vista-clase.component.css'
})
export class VistaClaseComponent {
   @Input() clase!: Clase;
   @Input() mostrarVolver: boolean = false;
   @Input() onEliminar?: (id: string) => void;
   @Input() onModificar?: (id: string) => void;
   todosLosMetodos: Metodo[] = [];

   constructor(private route: ActivatedRoute,
    private router: Router,
    private claseServicio: ClaseServicioService,
    private metodoServicio: MetodoServicio
  ) {}

  ngOnInit(): void {
    this.cargarTodosLosMetodos();   
      
    if (!this.clase && this.route.snapshot.paramMap.get('id')) {
      const id = this.route.snapshot.paramMap.get('id')!;
      this.mostrarVolver = true;
      this.claseServicio.obtenerPorId(id).subscribe({
        next: (clase) => this.clase = clase,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener clase.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });

      this.onEliminar = (id: string) => {
        this.claseServicio.eliminarClase(id).subscribe({
          next: () => {
            alert('Clase eliminado correctamente.');
            this.router.navigate(['/clase']);
          },
          error: (error) => {
            const mensaje = error.error?.mensaje || 'Error al eliminar.';
            const codigo = error.status;
            alert(`Error ${codigo}: ${mensaje}`);
          }
        });
      };

      this.onModificar = (id: string) => {
        this.router.navigate([`/clase/modificar/${id}`]);
      }
    }
  }

  private cargarTodosLosMetodos(): void {
    this.metodoServicio.obtenerMetodos().subscribe({
      next: (metodos) => {
        this.todosLosMetodos = metodos;
      },
      error: (err) => {
        console.error('Error al obtener los métodos:', err);
      }
    });
  }

  formatearMetodoInvocado(metodoInvocado: { id: string, nombre: string }): string {
    const metodo = this.todosLosMetodos.find(x => x.id === metodoInvocado.id);
    if (!metodo) return metodoInvocado.nombre;

    const mods = [
      metodo.esAbstract ? 'abstract' : '',
      metodo.esVirtual ? 'virtual' : '',
      metodo.esOverride ? 'override' : '',
      metodo.esEstatico ? 'static' : '',
      metodo.esSealed ? 'sealed' : ''
    ].filter(Boolean);

    const parametros = (metodo.parametros || [])
      .map(p => `${p.nombre}`)
      .join(', ');

    const acces = metodo.accesibilidad || 'public';
    const retorno = metodo.tipoRetorno || 'void';
    const modStr = mods.length ? ` (${mods.join(', ')})` : '';

    return `${acces} ${metodo.nombre}(${parametros})${modStr} : ${retorno}`;
  }
}
