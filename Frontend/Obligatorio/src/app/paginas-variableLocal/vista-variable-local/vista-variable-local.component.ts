import { Component, Input, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { VariableLocalService } from '../../servicios/variable-local.service';
import { VariableLocal } from '../../modelos/variableLocal/variable';
import { ActivatedRoute } from '@angular/router';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-variable-local',
  imports: [CommonModule, BotonVolverComponent],
  templateUrl: './vista-variable-local.component.html',
  styleUrl: './vista-variable-local.component.css'
})
export class VistaVariableLocalComponent implements OnInit{
  @Input() variable!: VariableLocal
  rutaEsDetalle = false;

  constructor(
    private route: ActivatedRoute,
    private variableService: VariableLocalService){}

    ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.rutaEsDetalle = true;
      this.variableService.obtenerPorId(id).subscribe({
        next: (variable) => this.variable = variable,
        error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener variable.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
      });
  }
    }
}
