import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-boton-modificar',
  imports: [],
  templateUrl: './boton-modificar.component.html',
  styleUrl: './boton-modificar.component.css'
})
export class BotonModificarComponent {
  @Input() onModificar!: (id: string) => void;
  @Input() id!: string;

  ejecutarModificar() {
    this.onModificar(this.id);
  }
}



