import { Component, Input } from '@angular/core';
import { Metodo } from '../../modelos/metodo/metodo';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BotonEliminarComponent } from '../../botones/boton-eliminar/boton-eliminar.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-metodo',
  imports: [BotonEliminarComponent, BotonVolverComponent],
  templateUrl: './vista-metodo.component.html',
  styleUrl: './vista-metodo.component.css'
})
export class VistaMetodoComponent {
  @Input() metodo!: Metodo;
  @Input() mostrarVolver: boolean = false;
  @Input() onEliminar?: (id: string) => void;
  todosLosMetodos: Metodo[] = [];


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private metodoServicio: MetodoServicio
  ) {}

  ngOnInit(): void {
    this.cargarTodosLosMetodos();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.metodoServicio.obtenerMetodoPorId(id).subscribe({
        next: (metodo) => this.metodo = metodo,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener método.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });

      this.onEliminar = (id: string) => {
      this.metodoServicio.eliminarMetodo(id).subscribe({
        next: () => {
        alert('Método eliminado correctamente.');
        this.router.navigate(['/metodos']);
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
