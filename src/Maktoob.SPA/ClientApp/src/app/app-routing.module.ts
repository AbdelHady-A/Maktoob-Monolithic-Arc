import { NgModule, Component } from '@angular/core';
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
    canActivateChild: [UnauthGuard],
  },
  {
    path: 'error',
    loadChildren: () => import('./error/error.module').then(
      (m) => m.ErrorModule
    )
  },
  {
    outlet: 'controlRouter',
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
  },
  {
    path: '',
    loadChildren: () =>
      import('./root/root.module').then(
        (m) => m.RootModule
      ),
    canLoad: [AuthGuard],
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