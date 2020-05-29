import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpInterceptorProviders } from './interceptors/interceptors';
import { ServiceProviders } from './services/services';
import { FacadeProviders } from './facades/facades'


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule
  ],
  providers: [
    HttpInterceptorProviders,
    ServiceProviders,
    FacadeProviders
  ],
  entryComponents: []
})
export class CoreModule { }
