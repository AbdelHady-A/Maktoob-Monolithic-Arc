import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { IOverlayFacade } from '../facades/overlay.facade';
import { Directionality } from '@angular/cdk/bidi';
import { OverlayType } from 'src/app/core/states/overlay.state';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})

export class NavComponent implements OnInit {

  @Output()
  SignOut = new EventEmitter<void>();

  vm$ = this.navOverlay.ViewModel$;
  constructor(private navOverlay: IOverlayFacade, private dir: Directionality) { }

  async _OpenSearch() {
    await this.navOverlay.Open('search', '');
  }

  _SignOut(): void {
    this.SignOut.emit();
  }

  async _OpenControl(overlayType: OverlayType,path: string): Promise<void> {
    await this.navOverlay.Open(overlayType, path);
  }

  _CloseControl(){
    this.navOverlay.Close();
  }

  SearchIconBack() {
    return this.dir.value === 'rtl' ? 'arrow_back' : 'arrow_forward';
  }
  ngOnInit(): void {
  }
}
