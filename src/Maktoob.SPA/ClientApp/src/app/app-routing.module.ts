import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { UnauthGuard } from './core/guards/unauth.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.module').then(
        (m) => m.AuthModule
      ),
    canLoad: [UnauthGuard],
    canActivate: [UnauthGuard],
    canActivateChild: [UnauthGuard]
  },
  {
    path: 'error',
    loadChildren: () => import('./error/error.module').then(
      (m) => m.ErrorModule
    )
  },
  {
    path: '',
    loadChildren: () =>
      import('./root/root.module').then(
        (m) => m.RootModule
      ),
    canLoad: [AuthGuard],
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard]
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'error/notfound',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
