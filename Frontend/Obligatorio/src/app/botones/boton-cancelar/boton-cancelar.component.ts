import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-boton-cancelar',
  imports: [],
  templateUrl: './boton-cancelar.component.html',
  styleUrl: './boton-cancelar.component.css'
})
export class BotonCancelarComponent {
  @Input() ruta: string = '/';

  constructor(private router: Router) {}

  irAlInicio(): void {
    this.router.navigate([this.ruta]);
  }
}
