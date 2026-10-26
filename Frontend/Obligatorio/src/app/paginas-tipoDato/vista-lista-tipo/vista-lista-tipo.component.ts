import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { VistaTipoDatoComponent } from '../vista-tipo-dato/vista-tipo-dato.component';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { TipoDato } from '../../modelos/tipo-dato';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vista-lista-tipo',
  imports: [ CommonModule, VistaTipoDatoComponent, BotonVolverComponent],
  templateUrl: './vista-lista-tipo.component.html',
  styleUrl: './vista-lista-tipo.component.css'
})
export class VistaListaTipoComponent {
  tiposDatos: TipoDato[] = [];

  constructor (private tipoDatoServicio: TipoDatoServicio, private router: Router) {}

  ngOnInit(): void {
    this.tipoDatoServicio.obtenerTiposDato().subscribe({
      next: tiposDatos => this.tiposDatos = tiposDatos,
      error: (error) => {
        const mensaje = error.error?.mensaje || 'Error al obtener tipos datos.';
        const codigo = error.status;
        alert(`Error ${codigo}: ${mensaje}`);
      }
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/tipoDatos', id]);
  }
}
