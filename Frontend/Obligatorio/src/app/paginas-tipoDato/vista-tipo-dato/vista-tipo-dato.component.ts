import { Component, Input } from '@angular/core';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';
import { TipoDato } from '../../modelos/tipo-dato';
import { TipoDatoServicio } from '../../servicios/tipo-dato-servicio.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-vista-tipo-dato',
  imports: [ BotonVolverComponent ],
  templateUrl: './vista-tipo-dato.component.html',
  styleUrl: './vista-tipo-dato.component.css'
})
export class VistaTipoDatoComponent {
  @Input() tipoDato!: TipoDato;
  @Input() mostrarVolver: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private tipoDatoServicio: TipoDatoServicio
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mostrarVolver = true;
      this.tipoDatoServicio.obtenerTipoPorId(id).subscribe({
        next: (tipoDato) => this.tipoDato = tipoDato,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener tipo de dato.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });      
    };
  }
}
