import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  if (req.url.includes(environment.authServiceUrl)) {
    return next(req);
  }

  const token = localStorage.getItem('token');

  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next(authReq).pipe(
      catchError(err => {
        if (err.status === 401) {
          router.navigate(['/login'], { replaceUrl: true });
        }

        return throwError(() => err);
      })
    );
  }

  return next(req);
};
