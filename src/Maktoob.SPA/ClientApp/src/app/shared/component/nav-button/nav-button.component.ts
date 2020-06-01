import { Component, OnInit, NgModule, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'gen-nav-button',
  templateUrl: './nav-button.component.html',
  styleUrls: ['./nav-button.component.scss']
})
export class NavButtonComponent implements OnInit {

  @Input()
  FirstIcon: string;
  @Input()
  SecondIcon: string;

  constructor() { }

  ngOnInit(): void {
  }
}

@NgModule({
  declarations: [NavButtonComponent],
  imports: [
    MatIconModule
  ],
  exports: [NavButtonComponent]
})
export class NavButtonModule { }
