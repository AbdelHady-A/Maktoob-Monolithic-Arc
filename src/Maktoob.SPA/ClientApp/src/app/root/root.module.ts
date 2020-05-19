import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RootRoutingModule } from './root-routing.module';
import { HomeComponent } from './home/home.component';
import { AccountModule } from './account/account.module';
import { NavComponent } from './nav/nav.component';
import { RootComponent } from './root.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    HomeComponent,
    NavComponent,
    RootComponent
  ],
  imports: [
    CommonModule,
    RootRoutingModule,
    MatButtonModule,
    MatToolbarModule,
    AccountModule
  ]
})
export class RootModule { }
