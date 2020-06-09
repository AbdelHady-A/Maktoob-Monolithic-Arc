import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { UnauthGuard } from './core/guards/unauth.guard';
import { SignInComponent } from './auth/signin/signin.component';
import { AuthComponent } from './auth/auth.component';
import { SignUpComponent } from './auth/signup/signup.component';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthComponent,
    // loadChildren: () =>
    //   import('./auth/auth.module').then(
    //     (m) => m.AuthModule
    //   ),
    // canLoad: [UnauthGuard],
    children: [
      { path: 'signup', component: SignUpComponent },
      { path: 'signin', component: SignInComponent },
      {
        path: '',
        redirectTo: 'signin',
        pathMatch: 'prefix'
      }
    ],
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