import { SupportedCultures } from "../enums/supported-cultures";

export class CultureInfo {
  public twoLetterISOLanguageName: string = '';
  public name: string = '';
  public englishName: string = ''

  constructor(culture: SupportedCultures) {

    switch (culture) {
      case SupportedCultures.italian: {
        this.twoLetterISOLanguageName = 'it-IT';
        this.name = 'Italiano';
        this.englishName = 'Italian';
        break;
      }

      case SupportedCultures.german: {
        this.twoLetterISOLanguageName = 'de-DE';
        this.name = 'Deutsch';
        this.englishName = 'German';
        break;
      }

      case SupportedCultures.english: {
        this.twoLetterISOLanguageName = 'en-EN';
        this.name = 'English';
        this.englishName = 'English';
        break;
      }

      case SupportedCultures.spanish: {
        this.twoLetterISOLanguageName = 'es-ES';
        this.name = 'Español';
        this.englishName = 'Spanish';
        break;
      }

      case SupportedCultures.french: {
        this.twoLetterISOLanguageName = 'fr-FR';
        this.name = 'Français';
        this.englishName = 'French';
        break;
      }
    }
  }
}
