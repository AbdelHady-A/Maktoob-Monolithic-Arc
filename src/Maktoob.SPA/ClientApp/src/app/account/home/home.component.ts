import { Component, OnInit } from '@angular/core';
import { IThemeFacade } from 'src/app/core/facades/theme.facade';
import { ThemeType } from 'src/app/core/states/theme.state';
import { Router } from '@angular/router';
import { IAuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-account-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(
    private themeFacade: IThemeFacade,
    private authService: IAuthService,
    private router: Router
  ) { }
  ngOnDestroy(): void {
  }
  theme$ = this.themeFacade.ViewModel$;

  ngOnInit(): void { }
  ChangeTheme(theme: ThemeType) {
    this.themeFacade.SetTheme(theme);
  }
  Navigate(path: string) {
    this.router.navigate([{ outlets: { controlRouter: ['account', path] } }], { skipLocationChange: true });
  }

  public SignOutAsync() {
    this.authService.SignOutAsync();
  }
}
