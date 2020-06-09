import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  /**
   * translate service injected here just to ensure its running in other parts of the app
   */
  constructor(
  ) { }

  ngOnInit(): void { }

  title = 'Maktoob';
}
