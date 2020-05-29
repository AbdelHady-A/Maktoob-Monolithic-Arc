import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ControlRoutingModule } from './control-routing.module';
import { ControlComponent } from './control.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
  declarations: [ControlComponent],
  imports: [
    CommonModule,
    ControlRoutingModule,
    MatToolbarModule,
    MatIconModule,
  ]
})

export class ControlModule { }
