import { Component, OnInit, OnDestroy } from '@angular/core';
import { IThemeFacade } from 'src/app/core/facades/theme.facade';
import { ThemeType } from 'src/app/core/states/theme.state';
import { TranslateService } from '@ngx-translate/core';
import { ILangFacade } from '../core/facades/lang.facade';
import { TranslateHttpLoaderFactory } from './account.module';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit, OnDestroy {

  constructor(
    private langFacade: ILangFacade,
    ) {
  }
  ngOnInit(): void {
  }
  ngOnDestroy(): void {
  }
}
