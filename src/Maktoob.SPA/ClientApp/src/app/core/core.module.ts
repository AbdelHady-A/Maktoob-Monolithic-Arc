import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpInterceptorProviders } from './interceptors/interceptors';
import { ServiceProviders } from './services/services';



@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    HttpInterceptorProviders,
    ServiceProviders
  ]
})
export class CoreModule { }
