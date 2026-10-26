import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-boton-simular',
  imports: [],
  templateUrl: './boton-simular.component.html',
  styleUrl: './boton-simular.component.css'
})
export class BotonSimularComponent {
  @Input() onCrear: () => void = () => {};

  ejecutarCrear() {
    this.onCrear();
  }

  @Input() disabled: boolean = false;
}
