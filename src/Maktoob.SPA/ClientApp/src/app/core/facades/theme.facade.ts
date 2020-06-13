import { Injectable, RendererFactory2, Renderer2, OnDestroy, OnInit, Inject } from '@angular/core';
import { IStorageService } from '../services/storage.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilKeyChanged } from 'rxjs/operators';
import { ThemeState, ThemeType } from '../states/theme.state';
import { ILinkeService } from '../services/link.service';
import { DOCUMENT } from '@angular/common';

@Injectable()
export abstract class IThemeFacade implements OnInit {
  abstract ngOnInit(): void;
  abstract SetTheme(theme: ThemeType): void;
  abstract DarkMode(on: boolean): void;
  abstract ViewModel$: Observable<ThemeState>;
}


@Injectable()
export class ThemeFacade implements IThemeFacade {
  private state: ThemeState = {
    ActiveTheme: 'light'
  };
  private store = new BehaviorSubject<ThemeState>(this.state);
  public ViewModel$ = this.store.asObservable().pipe(distinctUntilKeyChanged('ActiveTheme'));

  constructor(
    private storageService: IStorageService,
    @Inject(DOCUMENT) private document: Document
  ) {

  }
  ngOnInit(): void {
    this.document.body.classList.remove('dark-background');
    this.document.body.classList.remove('light-background');
    // initialize active language with the previously stored language
    let state = this.storageService.GetItem<ThemeState>('theme');
    if (!state) {
      state = {} as ThemeState;
    }
    if (state.DarkMode) {
      this.DarkMode(true);
    }
    if (state.ActiveTheme) {
      this.SetTheme(state.ActiveTheme);
    } else {
      state.ActiveTheme = this.state.ActiveTheme;
      this.storageService.SetItem('theme', state);
    }
    this.SetTheme(state.ActiveTheme);
  }

  DarkMode(on: boolean): void {
    const classList = this.document?.body.classList;
    if (on) {
      classList.add('dark-mode');
    } else {
      classList.remove('dark-mode');
    }
    this.updateState({ ...this.state, DarkMode: on });
  }

  SetTheme(theme: ThemeType): void {
    this.switchTheme(theme);
    this.updateState({ ...this.state, ActiveTheme: theme });
  }


  private updateState(state: ThemeState) {
    this.storageService.SetItem('theme', state);
    this.store.next(this.state = state);
  }

  private findStyle(theme: string) {

    const links = this.document.getElementsByTagName('link');
    for (const key in links) {
      if (links.hasOwnProperty(key)) {
        if (
          links[key].rel.indexOf('stylesheet') !== -1 &&
          links[key].title === theme
        ) {
          return true;
        }
      }
      return false;
    }
  }

  private switchTheme(theme: ThemeType) {
    if (theme && this.findStyle(theme)) {
      const links = this.document?.getElementsByTagName('link');
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
