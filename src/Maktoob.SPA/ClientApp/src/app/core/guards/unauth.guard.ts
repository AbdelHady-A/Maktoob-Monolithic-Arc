import { Injectable } from '@angular/core';
import { CanLoad, UrlSegment, Route, Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { IAuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UnauthGuard implements CanLoad, CanActivate, CanActivateChild {
  /**
   *
   */
  constructor(private router: Router, private authService: IAuthService) { }
  async canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const isAuthorized = await this.guard(childRoute.url);

    return isAuthorized;
  }
  private async guard(urlSegments: UrlSegment[]): Promise<boolean> {

    const isAuthorized = await this.authService.IsAuthorizedAsync();
    if (isAuthorized) {
      this.router.navigate(['']);
    }
    return !isAuthorized;
  }
  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const isAuthorized = await this.guard(route.url);

    return isAuthorized;
  }

  async canLoad(
    route: Route,
    segments: UrlSegment[]): Promise<boolean> {
    const isAuthorized = await this.guard(segments);

    return isAuthorized;
  }
}
