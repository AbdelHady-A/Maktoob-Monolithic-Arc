import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DebounceClickDirective } from './directive/debounce-click.directive';
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
