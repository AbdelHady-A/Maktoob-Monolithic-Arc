import { Component, OnInit } from '@angular/core';
import { IThemeFacade } from './core/facades/theme.facade';

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
    private themeFacade: IThemeFacade
  ) { }

  ngOnInit(): void { }

  title = 'Maktoob';
}
