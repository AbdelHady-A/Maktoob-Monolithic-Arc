import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { ILangFacade } from '../core/facades/lang.facade';
import { LangState } from '../core/states/lang.state';

@Component({
    selector: 'app-loader',
    template:
        `<mat-card *ngIf="(vm$ | async) as vm" dir="{{vm.ActiveLang.dir}}">
          <mat-card-header>
              <mat-card-title>
                {{ TranslatePath | translate }}
              </mat-card-title>
          </mat-card-header>
          <mat-card-content>
              <mat-progress-bar mode="indeterminate"></mat-progress-bar>
          </mat-card-content>
      </mat-card>`
})
export class LoaderComponent {
    @Input()
    public TranslatePath: string = '';
    vm$: Observable<LangState> = this.langFacade.ViewModel$;
    constructor(private langFacade: ILangFacade) { }
}