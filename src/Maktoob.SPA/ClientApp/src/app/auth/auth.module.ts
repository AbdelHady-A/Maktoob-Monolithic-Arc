import { NgModule } from '@angular/core';

import { AuthRoutingModule } from './auth-routing.module';
import { SignUpComponent } from './signup/signup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AuthComponent } from './auth.component';
import { ILangFacade, LangFacade } from '../core/facades/lang.facade';
import { FacadeProviders } from './facades/facades';
import { SignInComponent } from './signin/signin.component';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar'
import { OverlayModule } from '@angular/cdk/overlay';
import { LoaderComponent } from './loader.component';
import { ThrottleClickModule } from '../shared/directive/throttle-click.directive';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    SignUpComponent,
    AuthComponent,
    SignInComponent,
    LoaderComponent
  ],
  imports: [
    CommonModule,
    ThrottleClickModule,
    AuthRoutingModule,
    MatCardModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatButtonModule,
    FormsModule,
    MatProgressBarModule,
    ReactiveFormsModule,
    OverlayModule,
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
    { provide: ILangFacade, useClass: LangFacade },
    FacadeProviders
  ],
  entryComponents: [LoaderComponent]
})
export class AuthModule { }

export function TranslateHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/auth/', '.json');
}