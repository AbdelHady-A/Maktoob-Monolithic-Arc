import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RouterModule } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CoreModule } from './core/core.module';
import { ILangFacade, LangFacade } from './core/facades/lang.facade';


export function getBaseUrl() {
   return environment.API_BASE_URL;
}

export function TranslateHttpLoaderFactory(http: HttpClient) {
   return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
   declarations: [
      AppComponent,
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'serverApp' }),
      AppRoutingModule,
      BrowserAnimationsModule,
      HttpClientModule,
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
      TranslateModule.forRoot({
         extend: true,
         loader: {
            provide: TranslateLoader,
            useFactory: TranslateHttpLoaderFactory,
            deps: [HttpClient]
         },
         isolate: true
      }),
      RouterModule,
      CoreModule
   ],
   providers: [
      { provide: 'API_BASE_URL', useFactory: getBaseUrl, deps: [] },
      { provide: ILangFacade, useClass: LangFacade },

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
