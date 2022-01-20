import { Component } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { PageEvent } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { AuthorizeService } from "../../../../api-authorization/authorize.service";
import { TableViewBaseComponent } from "../../../common/base-components/table-view-base-component";
import { ProductsSectionRoutes } from "../../../common/enums/products-section-routes";
import { FilterDetails } from "../../../common/models/filter-details";
import { PaginationDetails } from "../../../common/models/pagination-details";
import { SortingDetails } from "../../../common/models/sorting-details";
import { IdentityService } from "../../../common/services/identity.service";
import { ProductCategoryService } from "../../../common/services/products-section/product-category.service";
import { ProductCategoryDto } from "../../../common/services/web-api-client";

@Component({
  selector: 'app-product-categories-table-view',
  templateUrl: './product-categories-table-view.component.html'
})
export class ProductCategoriesTableViewComponent extends TableViewBaseComponent<ProductCategoryDto>
{
  constructor(protected formBuilder: FormBuilder,
    protected authService: AuthorizeService,
    protected identityService: IdentityService,
    private productCategoryService: ProductCategoryService) {
    super(formBuilder, authService, identityService);

    this.initializeComponent(
      'Categorie',
      'Categorie di prodotti.',
      ProductsSectionRoutes.ProductCategoryEditor,
      ['Nome'],
      ['name', 'buttons']
    );
  }

  //#region HTML
  public onFilterOrSortChanged(): void {
    this.updatePage();
  }
  public onPageChanged(event: PageEvent): void {
    this.updatePageWithEvent(event);
  }
  public onDeleteItemRequest(item: ProductCategoryDto): void {
    if (item) {
      this.productCategoryService.deleteProductCategory(item.id, () => this.updatePage());
    }
  }
  //#endregion

  //#region Internal
  protected updatePageWithEvent(event: PageEvent): void {
    this.productCategoryService.getPaginatedListOfProductCategoryDto(new PaginationDetails(
      event ? event.pageIndex : this.paginator.pageIndex,
      event ? event.pageSize : this.paginator.pageSize),
      new SortingDetails(this.sort?.active, SortingDetails.DirectionFromString(this.sort?.direction)),
      [new FilterDetails(this.filterQuery(), 'name')],
      (result) => {
        if (result) {
          this.paginator.length = result.totalCount;
          this.tableData = new MatTableDataSource<ProductCategoryDto>(result.items);
        }
      }
    );
  }
  //#endregion
}
