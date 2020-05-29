import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle'
import { CxButtonModule } from 'src/app/shared/component/cx-button/cx-button.component';
import { MatDividerModule } from '@angular/material/divider'
import { CxSlideToggleModule } from 'src/app/shared/component/cx-slide-toggle/cx-slide-toggle.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ThemeModeComponent } from './theme-mode/theme-mode.component';
@NgModule({
  declarations: [AccountComponent, ThemeModeComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    MatSlideToggleModule,
    CxButtonModule,
    CxSlideToggleModule,
    MatDividerModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class AccountModule { }
