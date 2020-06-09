import { Injectable, OnDestroy, Renderer2, RendererFactory2 } from '@angular/core';
import { Overlay, OverlayRef, PositionStrategy } from '@angular/cdk/overlay';
import { Subscription, BehaviorSubject, Observable } from 'rxjs';
import { ComponentPortal } from '@angular/cdk/portal';
import { Router } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Directionality } from '@angular/cdk/bidi';
import { distinctUntilChanged } from 'rxjs/operators';
import { OverlayType, OverlayState } from 'src/app/core/states/overlay.state';

const SearchComponent = async () => import('../search/search.component').then(c => c.SearchComponent)


@Injectable()
export abstract class IOverlayFacade {
  abstract Open(overlayType: OverlayType, path: string): Promise<void>;
  abstract Close();
  abstract ViewModel$: Observable<OverlayState>;
}



@Injectable()
export class OverlayFacade implements IOverlayFacade, OnDestroy {
  private overlayRef: OverlayRef;
  private subscriptions: Subscription[] = [];
  private renderer: Renderer2;
  private listenerFn: () => void;

  private state: OverlayState = {};
  private store = new BehaviorSubject<OverlayState>(this.state);
  public ViewModel$ = this.store.asObservable().pipe(distinctUntilChanged());

  constructor(
    private overlay: Overlay,
    private router: Router,
    private breakpointObserver: BreakpointObserver,
    private rendrerFactory: RendererFactory2,
    private dir: Directionality
  ) {

    this.renderer = this.rendrerFactory.createRenderer(null, null);
    this.listenerFn = this.renderer.listen('window', 'click', (e) => {

      const classList = e.target.classList;
      let isOverlay = false;
      for (const c of classList) {
        if (c.startsWith("cdk-overlay")) {
          isOverlay = true;
          break
        }
      }
      if (isOverlay) {
        return;
      }
      if (this.state.Apparent) {
        this.Close();
      }
    });

    let sub = this.breakpointObserver.observe([
      Breakpoints.XSmall,
    ]).subscribe(() => {
      const positionStrategy = this.createPositionStrategy(this.state.ActiveOverlayType);
      this.overlayRef?.updatePositionStrategy(positionStrategy);
    });
    this.subscriptions = [...this.subscriptions, sub];

    sub = this.dir.change.subscribe(() => {
      const positionStrategy = this.createPositionStrategy(this.state.ActiveOverlayType);
      this.overlayRef?.updatePositionStrategy(positionStrategy);
    })
    this.subscriptions = [...this.subscriptions, sub];
  }

  public ngOnDestroy(): void {
    this.Close();
    if (this.listenerFn) {
      this.listenerFn();
    }
  }

  public Close() {
    if (this.state.Apparent) {
      this.overlayRef?.detach();
      this.unSubscribeAll();
      this.overlayRef?.dispose();
      this.overlayRef = null;
      this.updateState({ ActiveOverlayType: null, Apparent: false, ActivePath: null })
      this.ClosePanel();
    }
  }

  public async Open(overlayType: OverlayType, path: string): Promise<void> {
    if (overlayType !== this.state.ActiveOverlayType) {
      this.Close();
    }

    switch (overlayType) {
      case "control":
        this.OpenPanel(path);
        break;
      case "search":
        if (!this.overlayRef) {
          const positionStrategy = this.createPositionStrategy(overlayType);
          this.overlayRef = this.overlay.create({
            hasBackdrop: false,
            positionStrategy: positionStrategy,
            direction: document?.body?.dir as 'rtl' | 'ltr'
          })
        }
        if (!this.overlayRef?.hasAttached()) {
          this.overlayRef.attach(new ComponentPortal(await SearchComponent()));
        }
        break;
      default:
        break;
    }

    this.updateState({ ActiveOverlayType: overlayType, Apparent: true, ActivePath: path });
  }


  private unSubscribeAll() {
    this.subscriptions.forEach(sub => {
      sub.unsubscribe();
    })
    this.subscriptions = [];
  }

  private createPositionStrategy(overlayType: OverlayType): PositionStrategy {
    let positionStrategy = this.overlay.position().global();
    if (overlayType === 'search') {
      if (this.breakpointObserver.isMatched(Breakpoints.XSmall)) {
        positionStrategy = positionStrategy.top('56px');
      } else {
        positionStrategy = positionStrategy.top('64px');
      }
      if (document?.body.dir === 'rtl') {
        positionStrategy = positionStrategy.right('0')
      } else {
        positionStrategy = positionStrategy.left('0')
      }
    }
    return positionStrategy;
  }

  private updateState(state: OverlayState) {
    this.store.next(this.state = state);
  }

  private ClosePanel() {
    if (typeof window !== 'undefined') {
      const classList = document?.getElementById('Control').classList;
      if (classList.contains('active')) {
        classList.remove('active');
      }
    }
    this.router.navigate([{ outlets: { controlRouter: null } }], { skipLocationChange: true });
  }

  private OpenPanel(path: string) {
    if (typeof window !== 'undefined') {
      const classList = document?.getElementById('Control').classList;
      if (!classList.contains('active')) {
        classList.add('active');
      }
    }
    this.router.navigate([{ outlets: { controlRouter: [path] } }], { skipLocationChange: true })
  }
}
