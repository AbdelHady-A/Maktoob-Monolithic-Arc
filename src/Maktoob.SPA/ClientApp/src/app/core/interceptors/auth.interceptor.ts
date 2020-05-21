import { Injectable, Inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { IAuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private AUTH_HEADER = "Authorization";

  constructor(
    private authService: IAuthService,
    @Inject('API_BASE_URL') private API_BASE_URL
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (
      (request.url.startsWith(this.API_BASE_URL + 'auth')
        ||
        !request.url.startsWith(this.API_BASE_URL))
      &&
      !(request.url.endsWith('SignOut'))
    ) {
      // the requests that don't need authorization
      return next.handle(request);
    }



    if (this.authService.IsTokenExpired()) {
      // must update token before sending the request
      return this.authService.RefreshToken().pipe(
        switchMap(_ => {
          request = this.addAuthorizationToken(request);
          return next.handle(request)
        }),
      );
    } else {
      request = this.addAuthorizationToken(request);

      // send request don't need to update the token
      return next.handle(request);
    }

  }

  private addAuthorizationToken(request: HttpRequest<any>): HttpRequest<any> {
    // If we do not have a token yet then we should not set the header.
    // Here we could first retrieve the token from where we store it.
    const token = this.authService.AccessToken;
    if (!token) {
      return request;
    }
    // If you are calling an outside domain then do not add the token.
    if (!request.url.startsWith(this.API_BASE_URL)) {
      return request;
    }

    return request.clone({
      headers: request.headers.set(this.AUTH_HEADER, "Bearer " + token)
    });
  }
}
