import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountComponent } from './account.component';
import { SettingsComponent } from './settings/settings.component';
import { HomeComponent } from './home/home.component';
import { LanguageComponent } from './language/language.component';


const routes: Routes = [{
  path: '',
  component: AccountComponent,
  children: [
    {
      path: 'settings',
      component: SettingsComponent
    }, {
      path: 'language',
      component: LanguageComponent
    }, {
      path: '',
      component: HomeComponent
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
