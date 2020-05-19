import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { IStorageService } from '../services/storage.service';

@Injectable()
export class LangInterceptor implements HttpInterceptor {

  constructor(private sotrageSerivce: IStorageService) { }
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const langRequest = request.clone(
      {
        setHeaders:
        {
          'Accept-Language': this.sotrageSerivce.GetItem('lang') ?? 'en'
        }
      });
    return next.handle(langRequest);
  }
}
