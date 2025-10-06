import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class TenantInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const tenant = localStorage.getItem('tenant'); // recupero tenant dal login

    if (tenant) {
      const modifiedReq = req.clone({
        setHeaders: {
          'X-Tenant-ID': tenant
        }
      });
      return next.handle(modifiedReq);
    }

    return next.handle(req);
  }
}