import { Component } from '@angular/core';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { CommonModule } from '@angular/common';
import { VistaInterfazComponent } from '../vista-interfaz/vista-interfaz.component';
import { Interfaz } from '../../modelos/interfaz/interfaz';
import { InterfazServicio } from '../../servicios/interfaz-servicio.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vista-lista-interfaces',
  imports: [CommonModule, VistaInterfazComponent, BotonVolverComponent],
  templateUrl: './vista-lista-interfaces.component.html',
  styleUrl: './vista-lista-interfaces.component.css'
})
export class VistaListaInterfacesComponent {
  interfaces: Interfaz[] = [];

  constructor(private interfazServicio: InterfazServicio, private router: Router) {}

  ngOnInit(): void {
    this.interfazServicio.obtenerInterfaces().subscribe({
      next: interfaces => this.interfaces = interfaces,
      error: err => console.error('Error al obtener interfaces', err)
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/interfaces', id]);
  }
}
