import { Injectable, OnDestroy, Component, ElementRef, ViewChild, Renderer2, RendererFactory2 } from '@angular/core';
import { Overlay, OverlayRef, PositionStrategy } from '@angular/cdk/overlay';
import { Subscription, BehaviorSubject, Observable } from 'rxjs';
import { ComponentPortal } from '@angular/cdk/portal';
import { Router } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Directionality } from '@angular/cdk/bidi';
import { distinctUntilChanged } from 'rxjs/operators';
import { OverlayType, OverlayState } from 'src/app/core/states/overlay.state';

const SearchComponent = async () => import('../search/search.component').then(c => c.SearchComponent)

@Component({
  selector: 'app-control-router',
  template: `
  <div (click)="$event.stopPropagation()">
  <router-outlet name="controlRouter"></router-outlet>
  </div>
  `
})

export class ControlRouterComponent { }


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
    this.listenerFn = this.renderer.listen('window', 'click', () => {
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
      this.overlayRef.detach();
      this.unSubscribeAll();
      this.overlayRef.dispose();
      this.overlayRef = null;
      this.updateState({ ActiveOverlayType: null, Apparent: false, ActivePath: null })
    }
  }



  public async Open(overlayType: OverlayType, path: string): Promise<void> {

    if (overlayType !== this.state.ActiveOverlayType) {
      this.Close();
    }

    if (this.state.Apparent) {
      if (overlayType === 'control') {
        this.router.navigate([{ outlets: { controlRouter: ['control', path] } }], { skipLocationChange: true })
      }
      return
    }

    if (!this.overlayRef) {
      const positionStrategy = this.createPositionStrategy(overlayType);
      this.overlayRef = this.overlay.create({
        hasBackdrop: false,
        positionStrategy: positionStrategy
      })
    }
    if (overlayType === 'control') {

      if (!this.overlayRef.hasAttached()) {
        this.overlayRef.attach(new ComponentPortal(ControlRouterComponent));
      }

      this.router.navigate([{ outlets: { controlRouter: ['control', path] } }], { skipLocationChange: true });
    }

    if (overlayType === 'search') {

      if (!this.overlayRef?.hasAttached()) {
        this.overlayRef.attach(new ComponentPortal(await SearchComponent()));
      }
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
    } else {
      positionStrategy.top('56px');
    }
    if (overlayType === 'control') {
      if (this.dir.value === "rtl") {
        positionStrategy = positionStrategy.left('30px')
      } else {
        positionStrategy = positionStrategy.right('30px')
      }
    }
    return positionStrategy;
  }

  private updateState(state: OverlayState) {
    this.store.next(this.state = state);
  }
}
