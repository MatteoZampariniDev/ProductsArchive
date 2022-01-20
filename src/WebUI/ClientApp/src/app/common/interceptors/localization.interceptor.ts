import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LocalizationService } from "../services/localization.service";

@Injectable()
export class LocalizationInterceptor implements HttpInterceptor {

  constructor(private localizationService: LocalizationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var culture = this.localizationService.getCurrentCulture().twoLetterISOLanguageName;

    var modifiedReq = req.clone({
      headers: req.headers.set('Request-Culture', culture),
    });

    return next.handle(modifiedReq);
  }
}
