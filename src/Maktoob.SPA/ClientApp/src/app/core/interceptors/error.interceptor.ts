import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { GError } from '../results/error';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor() { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError((errorResponse: HttpErrorResponse) => {
      switch (errorResponse.status) {
        case 401: // unauthorized
          return throwError([{ Code: 'Unauthorized', Description: '' } as GError]);

        case 400: // bad request
          let errors = [];
          if (errorResponse.error.errors) {
            Object.entries(errorResponse.error.errors).forEach(error => {
              if (error[1] instanceof Array) {
                const e = (error[1] as []).map(desc => {
                  return { Code: error[0], Description: desc } as GError;
                })
                errors = [...errors, ...e]
              }
            });
          }
          if (errorResponse.error.Errors) {
            errors = [...errors, ...errorResponse.error.Errors];
          }
          return throwError(errors);

        case 404: // notfound
          return throwError([{ Code: 'Notfound', Description: '' } as GError]);
        case 415: // unsuported media type
          return throwError([{ Code: 'Unkown', Description: 'Unkown error occure' }])
        default:
          return throwError([{ Code: 'Unkown', Description: 'Unkown error occure' }])
      }
    }));
  }
}
