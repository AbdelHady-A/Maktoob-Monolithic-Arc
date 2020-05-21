import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignUpComponent } from './signup/signup.component';
import { AuthComponent } from './auth.component';
import { SignInComponent } from './signin/signin.component';
import { UnauthGuard } from '../core/guards/unauth.guard';

const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    canActivateChild: [UnauthGuard],
    children: [
      { path: 'signup', component: SignUpComponent },
      { path: 'signin', component: SignInComponent },
      {
        path: '',
        redirectTo: 'signin',
        pathMatch: 'prefix'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule { }
