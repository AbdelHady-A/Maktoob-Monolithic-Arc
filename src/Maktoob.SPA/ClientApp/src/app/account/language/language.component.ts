import { Component, OnInit } from '@angular/core';
import { ILangFacade } from 'src/app/core/facades/lang.facade';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent implements OnInit {
  vm$ = this.langFacade.ViewModel$;
  constructor(private langFacade: ILangFacade) { }

  ngOnInit(): void {
  }

  ChangeLang(langKey: string) {
    // console.log(lang)
    this.langFacade.ChangeLang(langKey)
  }
}
