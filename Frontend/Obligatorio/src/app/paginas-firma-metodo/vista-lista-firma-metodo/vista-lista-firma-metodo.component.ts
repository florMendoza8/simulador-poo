import { Component } from '@angular/core';
import { VistaFirmaMetodoComponent } from '../vista-firma-metodo/vista-firma-metodo.component';
import { CommonModule } from '@angular/common';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { FirmaMetodo } from '../../modelos/metodo/firma-metodo';
import { MetodoServicio } from '../../servicios/metodo-servicio.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vista-lista-firma-metodo',
  imports: [CommonModule, VistaFirmaMetodoComponent, BotonVolverComponent],
  templateUrl: './vista-lista-firma-metodo.component.html',
  styleUrl: './vista-lista-firma-metodo.component.css'
})
export class VistaListaFirmaMetodoComponent {
  firmaMetodos: FirmaMetodo[] = [];

  constructor(private metodoServicio: MetodoServicio, private router: Router) {}

  ngOnInit(): void {
    this.metodoServicio.obtenerFirmaMetodos().subscribe({
      next: firmaMetodos => this.firmaMetodos = firmaMetodos,
      error: err => console.error('Error al obtener firma de métodos', err)
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/firma-metodos', id]);
  }
}
