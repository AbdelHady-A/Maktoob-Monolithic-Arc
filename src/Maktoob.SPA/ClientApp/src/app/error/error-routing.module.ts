import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotfoundComponent } from './notfound/notfound.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';


const routes: Routes = [
  {
    path: 'notfound',
    component: NotfoundComponent
  },
  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  },
  {
    path: '**',
    pathMatch: 'prefix',
    redirectTo: 'notfound'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ErrorRoutingModule { }
