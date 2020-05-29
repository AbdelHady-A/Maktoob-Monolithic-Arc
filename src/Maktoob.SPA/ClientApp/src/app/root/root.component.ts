import { Component, OnInit } from '@angular/core';
import { IAuthService } from '../core/services/auth.service';
import { ILangFacade } from '../core/facades/lang.facade';
@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {
  vm$ = this.langFacade.ViewModel$;
  
  constructor(
    private authService: IAuthService,
    private langFacade: ILangFacade,
  ) { }

  ngOnInit(): void {
  }

  public SignOutAsync() {
    this.authService.SignOutAsync();
  }

}
