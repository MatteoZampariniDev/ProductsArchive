import { Injectable } from '@angular/core';
import { SupportedCultures } from '../enums/supported-cultures';
import { CultureInfo } from '../models/culture-info';
import { SessionStorageService } from './session-storage.service';

@Injectable({ providedIn: "root" })
export class LocalizationService {

  private defaultCulture = SupportedCultures.italian;

  constructor(private sessionStorageService: SessionStorageService) { }

  public setCurrentCulture(twoLetterISOLanguageName: SupportedCultures) {
    var tempCulture = new CultureInfo(twoLetterISOLanguageName);
    this.sessionStorageService.setData('CurrentCulture', tempCulture);
  }

  public getCurrentCulture(): CultureInfo {
    var tempData = this.sessionStorageService.getData('CurrentCulture');

    if (!tempData) {
      this.setCurrentCulture(this.defaultCulture);
      tempData = this.sessionStorageService.getData('CurrentCulture');
    }

    return JSON.parse(tempData);
  }
}

