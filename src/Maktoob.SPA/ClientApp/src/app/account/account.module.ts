import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle'
import { MatDividerModule } from '@angular/material/divider'
import { GenSlideToggleModule } from 'src/app/shared/component/slide-toggle/slide-toggle.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ThemeModeComponent } from './theme-mode/theme-mode.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ILangFacade, LangFacade } from 'src/app/core/facades/lang.facade';
import { SettingsComponent } from './settings/settings.component';
import { HomeComponent } from './home/home.component';
import { NavButtonModule } from 'src/app/shared/component/nav-button/nav-button.component';
import { LanguageComponent } from './language/language.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


export function TranslateHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/account/', '.json');
}


@NgModule({
  declarations: [
    AccountComponent,
    ThemeModeComponent,
    SettingsComponent,
    HomeComponent,
    LanguageComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    MatSlideToggleModule,
    GenSlideToggleModule,
    MatDividerModule,
    MatButtonModule,
    MatIconModule,
    NavButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule.forChild({
      extend: true,
      loader: {
        provide: TranslateLoader,
        useFactory: TranslateHttpLoaderFactory,
        deps: [HttpClient],
      },
      isolate: true
    })
  ],
  providers: [
    { provide: ILangFacade, useClass: LangFacade },
  ]
})
export class AccountModule { }
