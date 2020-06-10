import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Directionality } from '@angular/cdk/bidi';
import { OverlayType } from 'src/app/core/states/overlay.state';
import { IOverlayFacade, OverlayFacade } from '../facades/overlay.facade';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  providers: [
    { provide: IOverlayFacade, useClass: OverlayFacade }
  ]
})

export class NavBarComponent implements OnInit {

  vm$ = this.overlayFacade.ViewModel$;
  constructor(private overlayFacade: IOverlayFacade, private dir: Directionality) { }

  async _OpenSearch() {
    await this.overlayFacade.Open('search', '');
  }

  async _OpenControl(overlayType: OverlayType, path: string): Promise<void> {
    await this.overlayFacade.Open(overlayType, path);
  }

  _CloseControl() {
    this.overlayFacade.Close();
  }

  SearchIconBack() {
    return this.dir.value === 'rtl' ? 'arrow_back' : 'arrow_forward';
  }

  ngOnInit(): void {
  }
}
