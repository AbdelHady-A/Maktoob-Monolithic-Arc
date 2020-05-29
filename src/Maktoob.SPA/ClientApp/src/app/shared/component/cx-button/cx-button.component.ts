import { Component, OnInit, NgModule } from '@angular/core';

@Component({
  selector: 'app-cx-button',
  templateUrl: './cx-button.component.html',
  styleUrls: ['./cx-button.component.scss']
})
export class CxButtonComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

@NgModule({
  declarations: [CxButtonComponent],
  exports: [CxButtonComponent]
})
export class CxButtonModule { }