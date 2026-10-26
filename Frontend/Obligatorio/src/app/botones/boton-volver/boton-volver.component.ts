import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-boton-volver',
  standalone: true,
  templateUrl: './boton-volver.component.html',
})
export class BotonVolverComponent {
  @Input() ruta: string = '/';

  constructor(private router: Router) {}

  volver(): void {
    this.router.navigate([this.ruta]);
  }
}

