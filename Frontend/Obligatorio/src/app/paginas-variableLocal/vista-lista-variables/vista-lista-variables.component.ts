import { Component } from '@angular/core';
import { VariableLocalService} from '../../servicios/variable-local.service';
import { VariableLocal } from '../../modelos/variableLocal/variable';
import { Router } from '@angular/router';
import { VistaVariableLocalComponent} from '../vista-variable-local/vista-variable-local.component';
import { CommonModule } from '@angular/common';
import { BotonVolverComponent } from '../../botones/boton-volver/boton-volver.component';

@Component({
  selector: 'app-vista-lista-variables',
  imports: [CommonModule, VistaVariableLocalComponent, BotonVolverComponent],
  templateUrl: './vista-lista-variables.component.html',
  styleUrl: './vista-lista-variables.component.css'
})
export class VistaListaVariablesComponent {
  variables: VariableLocal[] = [];

  constructor(private variableServicio: VariableLocalService, private  router: Router ){}

   ngOnInit(): void {
    this.variableServicio.obtenerVariables().subscribe({
      next: variable => this.variables = variable,
      error: (error) => {
          const mensaje = error.error?.mensaje || 'Error al obtener variable.';
          const codigo = error.status;
          alert(`Error ${codigo}: ${mensaje}`);
        }
    });
  }

  verDetalle(id: string): void {
    this.router.navigate(['/variable', id]);
  }
}
