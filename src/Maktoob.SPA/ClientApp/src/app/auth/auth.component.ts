import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { LangState } from '../core/states/lang.state';
import { ILangFacade } from '../core/facades/lang.facade';

@Component({
  selector: 'app-root',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  vm$: Observable<LangState> = this.langFacade.ViewModel$;

  constructor(public langFacade: ILangFacade) { }
  ngOnInit(): void { }
}
