import { Component } from '@angular/core';
import { Parametro } from '../../modelos/parametro/parametro';
import { ParametroServicio } from '../../servicios/parametro-servicio.service';
import { Router } from '@angular/router';
import { VistaParametroComponent } from '../vista-parametro/vista-parametro.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vista-lista-parametros',
  imports: [ CommonModule, VistaParametroComponent, BotonVolverComponent ],
  templateUrl: './vista-lista-parametros.component.html',
  styleUrl: './vista-lista-parametros.component.css'
})
export class VistaListaParametrosComponent {
  parametros: Parametro[] = [];

  constructor(private parametroServicio: ParametroServicio, private router: Router) {}

  ngOnInit(): void {
    this.parametroServicio.obtenerParametros().subscribe({
      next: parametros => this.parametros = parametros,
      error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener parámetro.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/parametros', id]);
  }
}
