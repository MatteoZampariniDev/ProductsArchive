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
import { ProductSizeService } from "../../../common/services/products-section/product-size.service";
import { ProductSizeDto } from "../../../common/services/web-api-client";


@Component({
  selector: 'app-product-sizes-table-view',
  templateUrl: './product-sizes-table-view.component.html'
})
export class ProductSizesTableViewComponent extends TableViewBaseComponent<ProductSizeDto>
{
  constructor(protected formBuilder: FormBuilder,
    protected authService: AuthorizeService,
    protected identityService: IdentityService,
    private productSizeService: ProductSizeService) {
    super(formBuilder, authService, identityService);

    this.initializeComponent(
      'Formati',
      'Tutti i formati disponibili.',
      ProductsSectionRoutes.ProductSizeEditor,
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

  public onDeleteItemRequest(item: ProductSizeDto): void {
    if (item) {
      this.productSizeService.deleteProductSize(item.id, () => this.updatePage());
    }
  }
  //#endregion

  //#region Internal
  protected updatePageWithEvent(event: PageEvent): void {
    this.productSizeService.getPaginatedListOfProductSizeDto(new PaginationDetails(
      event ? event.pageIndex : this.paginator.pageIndex,
      event ? event.pageSize : this.paginator.pageSize),
      new SortingDetails(this.sort?.active, SortingDetails.DirectionFromString(this.sort?.direction)),
      [new FilterDetails(this.filterQuery(), 'name')],
      (result) => {
        if (result) {
          this.paginator.length = result.totalCount;
          this.tableData = new MatTableDataSource<ProductSizeDto>(result.items);
        }
      }
    );
  }
  //#endregion
}
