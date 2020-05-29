import { Injectable, RendererFactory2, Renderer2 } from '@angular/core';
import { IStorageService } from '../services/storage.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilKeyChanged } from 'rxjs/operators';
import { State, ThemeType } from '../states/theme.state';

@Injectable()
export abstract class IThemeFacade {
  abstract SetTheme(theme: ThemeType): void;
  abstract ViewModel$: Observable<State>;
}


@Injectable()
export class ThemeFacade implements IThemeFacade {
  private state: State = {
    ActiveTheme: 'light'
  };
  private store = new BehaviorSubject<State>(this.state);
  public ViewModel$ = this.store.asObservable().pipe(distinctUntilKeyChanged('ActiveTheme'));

  private renderer: Renderer2;
  constructor(
    private rendererFactory: RendererFactory2,
    private storageService: IStorageService
  ) {
    this.renderer = this.rendererFactory.createRenderer(null, null);
    // initialize active language with the previously stored language
    let theme = this.storageService.GetItem<ThemeType>('theme');
    if (theme) {
      this.SetTheme(theme);
    } else {
      theme = this.state.ActiveTheme;
      this.storageService.SetItem('theme', theme);
    }
  }

  SetTheme(theme: ThemeType): void {
    const classList = document?.body.classList;

    if (classList.contains(theme + '-theme')) {
      return
    }
    classList.forEach(c => {
      if (c.endsWith('-theme')) {
        this.renderer.removeClass(document?.body, c);
      }
    });

    this.renderer.addClass(document?.body, theme + '-theme');
    this.updateState({ ...this.state, ActiveTheme: theme });
  }


  private updateState(state: State) {
    this.storageService.SetItem('theme', state.ActiveTheme);
    this.store.next(this.state = state);
  }
}
