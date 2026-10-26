import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-boton-crear',
  imports: [],
  templateUrl: './boton-crear.component.html',
  styleUrl: './boton-crear.component.css'
})
export class BotonCrearComponent {
  @Input() onCrear: () => void = () => {};

  ejecutarCrear() {
    this.onCrear();
  }

  @Input() disabled: boolean = false;
}
