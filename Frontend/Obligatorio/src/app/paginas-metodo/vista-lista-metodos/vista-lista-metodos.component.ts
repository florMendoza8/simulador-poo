import { Component } from '@angular/core';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { Metodo } from '../../modelos/metodo/metodo';
import { Router } from '@angular/router';
import { VistaMetodoComponent } from '../vista-metodo/vista-metodo.component';
import { CommonModule } from '@angular/common';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-lista-metodos',
  imports: [ CommonModule, VistaMetodoComponent, BotonVolverComponent ],
  templateUrl: './vista-lista-metodos.component.html',
  styleUrl: './vista-lista-metodos.component.css'
})
export class VistaListaMetodosComponent {
  metodos: Metodo[] = [];

  constructor(private metodoServicio: MetodoServicio, private router: Router) {}

  ngOnInit(): void {
    this.metodoServicio.obtenerMetodos().subscribe({
      next: metodos => this.metodos = metodos,
      error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener método.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/metodos', id]);
  }
}
