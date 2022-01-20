import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { ProductsSectionRoutes } from '../common/enums/products-section-routes';
import { SupportedCultures } from '../common/enums/supported-cultures';
import { isNullOrWhiteSpaces } from '../common/helpers/string-extensions';
import { CultureInfo } from '../common/models/culture-info';
import { IdentityService } from '../common/services/identity.service';
import { LocalizationService } from '../common/services/localization.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  constructor(private localizationService: LocalizationService,
    private authService: AuthorizeService,
    private identityService: IdentityService) { }

  //#region HTML
  public currentCulture: CultureInfo = this.localizationService.getCurrentCulture();
  public isAuthenticated: boolean = false;
  public role: string = '';

  public enLang = SupportedCultures.english;
  public frLang = SupportedCultures.french;
  public deLang = SupportedCultures.german;
  public itLang = SupportedCultures.italian;
  public esLang = SupportedCultures.spanish;

  public categoriesRoute = ProductsSectionRoutes.ProductCategoriesTableView;
  public groupsRoute = ProductsSectionRoutes.ProductGroupsTableView;
  public productsRoute = ProductsSectionRoutes.ProductsTableView;
  public sizesRoute = ProductsSectionRoutes.ProductSizesTableView;

  public isExpanded = false;

  public collapse() {
    this.isExpanded = false;
  }

  public toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public setCulture(culture: SupportedCultures) {
    this.localizationService.setCurrentCulture(culture);
    window.location.reload();
  }

  public hasRole(requiredRole: string): boolean {
    return this.isAuthenticated && this.role === requiredRole;
  }
  //#endregion

  //#region Internal
  ngOnInit(): void {
    this.authService.isAuthenticated()
      .subscribe(
        (v) => {
          this.isAuthenticated = v;

          if (this.isAuthenticated) {

            this.identityService.getUserId((userId) => {

              if (!isNullOrWhiteSpaces(userId)) {

                this.identityService.getRole(userId, (role) => {

                  this.role = role;
                })
              }
            })
          }

        });
  }
  //#endregion
}
