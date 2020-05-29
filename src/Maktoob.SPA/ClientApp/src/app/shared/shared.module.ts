import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThrottleClickDirective } from './directive/throttle-click.directive';



@NgModule({
  declarations: [ThrottleClickDirective],
  imports: [

  ],
  exports: [
    CommonModule,
    ThrottleClickDirective
  ],
})
export class SharedModule { }
