import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RootRoutingModule } from './root-routing.module';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { RootComponent } from './root.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ServiceProviders } from '../core/services/services';
import { ILangFacade, LangFacade } from '../core/facades/lang.facade';

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
  ],
  providers: [
    ServiceProviders,
    { provide: ILangFacade, useClass: LangFacade },
  ]
})
export class RootModule { }
