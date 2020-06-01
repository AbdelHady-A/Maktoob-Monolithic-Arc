import { Component, OnInit } from '@angular/core';
import { IAuthService } from '../core/services/auth.service';
import { ILangFacade } from '../core/facades/lang.facade';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

  constructor(
    private langFacade: ILangFacade
  ) {
  }

  ngOnInit(): void {
  }
}
