import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-boton-eliminar',
  imports: [],
  templateUrl: './boton-eliminar.component.html',
  styleUrl: './boton-eliminar.component.css'
})
export class BotonEliminarComponent {

  @Input() onEliminar!: (id: string) => void;
  @Input() id!: string;

  ejecutarEliminar() {
    this.onEliminar(this.id);
  }
}
