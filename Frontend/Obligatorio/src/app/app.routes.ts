import { Routes } from '@angular/router';
import { PaginaInicioComponent } from './pagina-inicio/pagina-inicio.component';
import { CrearMetodoComponent } from './paginas-metodo/crear-metodo/crear-metodo.component';
import { CrearVariablelocalComponent } from './paginas-variableLocal/crear-variablelocal/crear-variablelocal.component';
import { CrearParametroComponent } from './paginas-parametro/crear-parametro/crear-parametro.component';
import { CrearAtributoComponent } from './paginas-atributo/crear-atributo/crear-atributo.component';
import { VistaAtributoComponent } from './paginas-atributo/vista-atributo/vista-atributo.component';
import { CrearInterfazComponent } from './paginas-interfaz/crear-interfaz/crear-interfaz.component';
import { VistaInterfazComponent } from './paginas-interfaz/vista-interfaz/vista-interfaz.component';
import { VistaListaMetodosComponent } from './paginas-metodo/vista-lista-metodos/vista-lista-metodos.component';
import { VistaMetodoComponent } from './paginas-metodo/vista-metodo/vista-metodo.component';
import { VistaVariableLocalComponent } from './paginas-variableLocal/vista-variable-local/vista-variable-local.component';
import { VistaListaVariablesComponent } from './paginas-variableLocal/vista-lista-variables/vista-lista-variables.component';
import { VistaListaParametrosComponent } from './paginas-parametro/vista-lista-parametros/vista-lista-parametros.component';
import { VistaParametroComponent } from './paginas-parametro/vista-parametro/vista-parametro.component';
import { CrearClaseComponent } from './pagina-clase/crear-clase/crear-clase.component';
import { VistaClaseComponent } from './pagina-clase/vista-clase/vista-clase/vista-clase.component';
import { VistaListaClaseComponent } from './pagina-clase/vista-lista/vista-lista-clase/vista-lista-clase.component';
import { CrearSimulacionComponent } from './paginas-simulacion/crear-simulacion/crear-simulacion.component';
import { VistaListaAtributosComponent } from './paginas-atributo/vista-lista-atributos/vista-lista-atributos.component';
import { ModificarAtributoComponent } from './paginas-atributo/modificar-atributo/modificar-atributo.component';
import { CrearFirmaMetodoComponent } from './paginas-firma-metodo/crear-firma-metodo/crear-firma-metodo.component';
import { VistaListaInterfacesComponent } from './paginas-interfaz/vista-lista-interfaces/vista-lista-interfaces.component';
import { VistaListaFirmaMetodoComponent } from './paginas-firma-metodo/vista-lista-firma-metodo/vista-lista-firma-metodo.component';
import { VistaFirmaMetodoComponent } from './paginas-firma-metodo/vista-firma-metodo/vista-firma-metodo.component';
import { ModificarClaseComponent } from './pagina-clase/modificar-clase/modificar-clase/modificar-clase.component';
import { VistaListaTipoComponent } from './paginas-tipoDato/vista-lista-tipo/vista-lista-tipo.component';
import { VistaTipoDatoComponent } from './paginas-tipoDato/vista-tipo-dato/vista-tipo-dato.component';

export const routes: Routes = [
  { path: '', component: PaginaInicioComponent },
  { path: "variable", component: VistaListaVariablesComponent},
  { path: "variable/crear", component: CrearVariablelocalComponent},
  { path: "variable/:id", component: VistaVariableLocalComponent},
  { path: "metodos", component: VistaListaMetodosComponent},
  { path: 'metodos/crear', component: CrearMetodoComponent },
  { path: 'metodos/:id', component: VistaMetodoComponent },
  { path: 'firma-metodos', component: VistaListaFirmaMetodoComponent },
  { path: 'firma-metodos/:id', component: VistaFirmaMetodoComponent },
  { path: 'firma-metodo/crear', component: CrearFirmaMetodoComponent },
  { path: "simulaciones/crear", component: CrearSimulacionComponent },
  { path: "interfaces", component: VistaListaInterfacesComponent },
  { path: "interfaces/:id", component: VistaInterfazComponent },
  { path: "interfaz/crear", component: CrearInterfazComponent },
  { path: "atributos", component: VistaListaAtributosComponent },
  { path: "atributos/:id", component: VistaAtributoComponent },
  { path: 'atributo/crear', component: CrearAtributoComponent},
  { path: 'atributos/modificar/:id', component: ModificarAtributoComponent },
  { path: "parametros", component: VistaListaParametrosComponent },
  { path: 'parametros/crear', component: CrearParametroComponent },
  { path: "parametros/:id", component: VistaParametroComponent },
  { path: "clase/crear", component: CrearClaseComponent},
  { path: 'clase/modificar/:id', component: ModificarClaseComponent },
  { path: "clase/:id", component: VistaClaseComponent},
  { path: "clase", component: VistaListaClaseComponent},
  { path: "tipoDatos", component: VistaListaTipoComponent},
  { path: "tipoDatos/:id", component: VistaTipoDatoComponent}
];
