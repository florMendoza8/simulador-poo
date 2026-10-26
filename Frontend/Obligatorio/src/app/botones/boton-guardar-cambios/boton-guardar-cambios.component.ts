import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-boton-guardar-cambios',
  imports: [],
  templateUrl: './boton-guardar-cambios.component.html',
  styleUrl: './boton-guardar-cambios.component.css'
})
export class BotonGuardarCambiosComponent {
  @Input() onModificar: () => void = () => {};

  ejecutarGuardarCambios() {
    this.onModificar();
  }

  @Input() disabled: boolean = false;
}
