import { Component, OnInit } from '@angular/core';

import { ILangFacade } from '../core/facades/lang.facade';

@Component({
  selector: 'app-root',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  vm$ = this.langFacade.ViewModel$;

  constructor(public langFacade: ILangFacade) { }
  ngOnInit(): void { }
  ChangeLang(langKey: string) {
    this.langFacade.ChangeLang(langKey)
  }
}
