import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RootRoutingModule } from './root-routing.module';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RootComponent } from './root.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu'
import { MatCardModule } from '@angular/material/card';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { NavBoxComponent } from './nav-box/nav-box.component';
import { SearchBoxComponent } from './search/search-box/search-box.component';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ILangFacade, LangFacade } from '../core/facades/lang.facade';
import { FacadeProviders } from './facades/facades';
import { ThrottleClickModule } from '../shared/directive/throttle-click.directive';


export function TranslateHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/root/', '.json');
}


@NgModule({
  declarations: [
    HomeComponent,
    NavBarComponent,
    RootComponent,
    NavBoxComponent,
    SearchBoxComponent
  ],
  imports: [
    CommonModule,
    ThrottleClickModule,
    RootRoutingModule,
    MatButtonModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatCardModule,
    TranslateModule.forChild({
      extend: true,
      loader: {
        provide: TranslateLoader,
        useFactory: TranslateHttpLoaderFactory,
        deps: [HttpClient]
      },
      isolate: true
    })
  ],
  providers: [
    FacadeProviders,
    { provide: ILangFacade, useClass: LangFacade },
  ]
})
export class RootModule { }
