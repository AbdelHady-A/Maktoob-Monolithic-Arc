import { Component, OnInit, OnDestroy } from '@angular/core';
import { IThemeFacade } from 'src/app/core/facades/theme.facade';
import { ThemeType } from 'src/app/core/states/theme.state';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit, OnDestroy {

  constructor(private themeFacade: IThemeFacade) { }
  ngOnDestroy(): void {
  }
  theme$ = this.themeFacade.ViewModel$;

  ngOnInit(): void {
  }
  ChangeTheme(theme: ThemeType) {
    this.themeFacade.SetTheme(theme);
  }
}
