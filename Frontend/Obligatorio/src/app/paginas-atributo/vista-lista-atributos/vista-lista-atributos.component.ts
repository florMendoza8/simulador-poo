import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { VistaAtributoComponent } from '../vista-atributo/vista-atributo.component';
import { Atributo } from '../../modelos/atributo/atributo';
import { AtributoServicio } from '../../servicios/atributo-servicio.service';

@Component({
  selector: 'app-vista-lista-atributos',
  imports: [CommonModule, VistaAtributoComponent, BotonVolverComponent],
  templateUrl: './vista-lista-atributos.component.html',
  styleUrl: './vista-lista-atributos.component.css'
})
export class VistaListaAtributosComponent {
  atributos: Atributo[] = [];

  constructor(private atributoServicio: AtributoServicio, private router: Router) {}

  ngOnInit(): void {
    this.atributoServicio.obtenerAtributos().subscribe({
      next: atributos => this.atributos = atributos,
      error: (error) => {
        const mensaje = error.error?.mensaje || 'Error al obtener atributos.';
        const codigo = error.status;
        alert(`Error ${codigo}: ${mensaje}`);
      }
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/atributos', id]);
  }
}
