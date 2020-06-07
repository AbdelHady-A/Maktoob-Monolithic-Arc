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

  constructor(
    private storageService: IStorageService
  ) {
    // initialize active language with the previously stored language
    let theme = this.storageService.GetItem<ThemeType>('theme');
    if (theme) {
      this.SetTheme(theme);
    } else {
      theme = this.state.ActiveTheme;
      this.storageService.SetItem('theme', theme);
    }
    this.SetTheme(theme);
  }

  SetTheme(theme: ThemeType): void {
    this.switchTheme(theme);
    this.updateState({ ...this.state, ActiveTheme: theme });
  }


  private updateState(state: State) {
    this.storageService.SetItem('theme', state.ActiveTheme);
    this.store.next(this.state = state);
  }

  private findStyle(theme: string) {
    const links = document.getElementsByTagName('link');
    for (const key in links) {
      if (links.hasOwnProperty(key)) {
        if (
          links[key].rel.indexOf('stylesheet') !== -1 &&
          links[key].title === theme
        ) {
          return true;
        }
      }
    }
    return false;
  }

  private switchTheme(theme: ThemeType) {
    if (theme && this.findStyle(theme)) {
      const links = document.getElementsByTagName('link');
      for (const key in links) {
        if (links.hasOwnProperty(key)) {
          const link = links[key];
          if (link.rel.indexOf('stylesheet') !== -1 && link.title) {
            if (link.title === theme) {
              link.disabled = false;
            } else {
              link.disabled = true;
            }
          }
        }
      }
    }
  }

}
