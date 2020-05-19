import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LangFacade, ILangFacade } from './core/facades/lang.facade';
import { CoreModule } from './core/core.module';


export function TranslateHttpLoaderFactory(http: HttpClient) {
   return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
   declarations: [
      AppComponent,
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      BrowserAnimationsModule,
      HttpClientModule,
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: true }),
      TranslateModule.forRoot({
         defaultLanguage: 'en',
         loader: {
            provide: TranslateLoader,
            useFactory: TranslateHttpLoaderFactory,
            deps: [HttpClient]
         }
      }),
      CoreModule,
   ],
   providers: [
      { provide: ILangFacade, useClass: LangFacade }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
