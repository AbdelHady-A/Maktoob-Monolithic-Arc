import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Directionality } from '@angular/cdk/bidi';
import { OverlayType } from 'src/app/core/states/overlay.state';
import { IOverlayFacade } from '../facades/overlay.facade';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})

export class NavBarComponent implements OnInit {

  vm$ = this.navOverlay.ViewModel$;
  constructor(private navOverlay: IOverlayFacade, private dir: Directionality) { }

  async _OpenSearch() {
    await this.navOverlay.Open('search', '');
  }

  async _OpenControl(overlayType: OverlayType, path: string): Promise<void> {
    await this.navOverlay.Open(overlayType, path);
  }

  _CloseControl() {
    this.navOverlay.Close();
  }

  SearchIconBack() {
    return this.dir.value === 'rtl' ? 'arrow_back' : 'arrow_forward';
  }
  
  ngOnInit(): void {
  }
}
