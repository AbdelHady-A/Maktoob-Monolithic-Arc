import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RootRoutingModule } from './root-routing.module';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { RootComponent } from './root.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ILangFacade, LangFacade } from '../core/facades/lang.facade';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu'
import { FacadeProviders } from './facades/facades';
import { MatCardModule } from '@angular/material/card';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoaderFactory } from '../app.module';
import { HttpClient } from '@angular/common/http';
import { ControlRouterComponent } from './facades/overlay.facade';
import { ControlBoxComponent } from './control-box/control-box.component';
import { SearchBoxComponent } from './search-box/search-box.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    HomeComponent,
    NavComponent,
    RootComponent,
    ControlRouterComponent,
    ControlBoxComponent,
    SearchBoxComponent
  ],
  imports: [
    CommonModule,
    RootRoutingModule,
    MatButtonModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatCardModule,
    SharedModule,
    TranslateModule.forChild({
      extend: true,
      loader: {
        provide: TranslateLoader,
        useFactory: TranslateHttpLoaderFactory,
        deps: [HttpClient]
      },
      isolate: true,
      defaultLanguage: 'en'
    })
  ],
  providers: [
    FacadeProviders,
    { provide: ILangFacade, useClass: LangFacade },
  ]
})
export class RootModule { }
