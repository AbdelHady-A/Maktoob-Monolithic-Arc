import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { environment } from 'src/environments/environment';
import { CoreModule } from './core/core.module';
import { ThrottleClickModule } from './shared/directive/throttle-click.directive';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignUpComponent } from './auth/signup/signup.component';
import { AuthComponent } from './auth/auth.component';
import { SignInComponent } from './auth/signin/signin.component';
import { FacadeProviders } from './auth/facades/facades';
import { ILangFacade, LangFacade } from './core/facades/lang.facade';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';


export function getBaseUrl() {
   return environment.API_BASE_URL;
}

export function TranslateHttpLoaderFactory(http: HttpClient) {
   return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
   declarations: [
      AppComponent,
      SignUpComponent,
      AuthComponent,
      SignInComponent,
   ],
   imports: [
      ThrottleClickModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
      TranslateModule.forRoot({
         loader: {
            provide: TranslateLoader,
            useFactory: TranslateHttpLoaderFactory,
            deps: [HttpClient]
         },
         isolate: true
      }),
      CoreModule
   ],
   providers: [
      FacadeProviders,
      { provide: 'API_BASE_URL', useFactory: getBaseUrl, deps: [] },
      { provide: ILangFacade, useClass: LangFacade }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
