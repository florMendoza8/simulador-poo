import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Clase } from '../../../modelos/clase/clase';
import { ClaseServicioService } from '../../../servicios/clase-servicio.service';
import { Router } from '@angular/router';
import { BotonVolverComponent } from '../../../botones/boton-volver/boton-volver.component';
import{VistaClaseComponent} from '../../vista-clase/vista-clase/vista-clase.component';

@Component({
  selector: 'app-vista-lista-clase',
  standalone: true,
  imports: [CommonModule, BotonVolverComponent, VistaClaseComponent],
  templateUrl: './vista-lista-clase.component.html',
  styleUrl: './vista-lista-clase.component.css'
})
export class VistaListaClaseComponent {
  clases: Clase[] = [];

  constructor(private claseServicio: ClaseServicioService, private router: Router){}

  ngOnInit(): void {
    this.claseServicio.obtenerTodas().subscribe({
      next: clases => this.clases = clases,
      error: (error) => {
        const mensaje = error.error?.mensaje || 'Error al obtener clases.';
        const codigo = error.status;
        alert(`Error ${codigo}: ${mensaje}`);
      }
    })
  }

  verDetalle(id: string): void {
    this.router.navigate(['/clase', id]);
  }
}
