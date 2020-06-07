import { Component, OnInit } from '@angular/core';
import { ILangFacade, LangFacade } from 'src/app/core/facades/lang.facade';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss'],
  providers: [
    { provide: ILangFacade, useClass: LangFacade }
  ]
})
export class LanguageComponent implements OnInit {
  vm$ = this.langFacade.ViewModel$;
  constructor(
    private langFacade: ILangFacade
  ) { }
  langForm: FormGroup;
  ngOnInit(): void {
    this.langForm = this.langFacade.BuildForm();
  }

  ChangeLang(langKey: string) {
    this.langFacade.ChangeLang(langKey)
  }

  Save() {
    this.langFacade.Save();
  }
}
