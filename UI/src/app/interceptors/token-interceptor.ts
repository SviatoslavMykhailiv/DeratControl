import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { UserInfoService } from '../services/user-info.service';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private userInfoService: UserInfoService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.userInfoService.getToken();
    const lang = localStorage.getItem("lang") || "uk-UA";

    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'Accept-Language': lang
      }
    });

    return next.handle(request).pipe(catchError((error: HttpErrorResponse) => {
      if (error && error.status == 401) {
        this.router.navigate(['login']);
      }

      return throwError(error);
    }));
  }
}
