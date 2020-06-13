import { Component, OnInit, OnDestroy } from '@angular/core';
import { IAuthService } from '../core/services/auth.service';
import { ILangFacade } from '../core/facades/lang.facade';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { ILinkeService } from '../core/services/link.service';
import { IThemeFacade } from '../core/facades/theme.facade';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit, OnDestroy {

  constructor(
    private langFacade: ILangFacade,
    private linkService: ILinkeService,
    // private themeFacade: IThemeFacade
  ) {
  }
  ngOnDestroy(): void {
  }
  ngOnInit(): void {
    this.linkService.AddTag({ id: 'icons', rel: 'stylesheet', href: 'https://fonts.googleapis.com/icon?family=Material+Icons|Material+Icons+Outlined|Material+Icons+Two+Tone|Material+Icons+Round|Material+Icons+Sharp' });
  }
}
