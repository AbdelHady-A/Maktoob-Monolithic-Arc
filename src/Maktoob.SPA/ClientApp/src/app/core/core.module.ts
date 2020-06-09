import { NgModule } from '@angular/core';
import { HttpInterceptorProviders } from './interceptors/interceptors';
import { ServiceProviders } from './services/services';
import { FacadeProviders } from './facades/facades'


@NgModule({
  providers: [
    HttpInterceptorProviders,
    ServiceProviders,
    FacadeProviders
  ]
})
export class CoreModule { }
