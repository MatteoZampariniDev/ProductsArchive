import { Component, } from "@angular/core";
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
import { ProductService } from "../../../common/services/products-section/product.service";
import { ProductDto } from "../../../common/services/web-api-client";


@Component({
  selector: 'app-products-table-view',
  templateUrl: './products-table-view.component.html'
})
export class ProductsTableViewComponent extends TableViewBaseComponent<ProductDto>
{
  constructor(protected formBuilder: FormBuilder,
    protected authService: AuthorizeService,
    protected identityService: IdentityService,
    private productService: ProductService) {
    super(formBuilder, authService, identityService);

    this.initializeComponent(
      'Prodotti',
      'Prodotti singoli.',
      ProductsSectionRoutes.ProductEditor,
      ['ID Prodotto', 'Nome Prodotto', 'Formato', 'Peso Netto'],
      ['productId', 'group.name', 'size.name', 'netWeight', 'buttons']
    );
  }

  //#region HTML
  public onFilterOrSortChanged(): void {
    this.updatePage();
  }

  public onPageChanged(event: PageEvent): void {
    this.updatePageWithEvent(event);
  }

  public onDeleteItemRequest(item: ProductDto): void {
    if (item) {
      this.productService.deleteProduct(item.id, () => this.updatePage());
    }
  }
  //#endregion

  //#region Internal
  protected updatePageWithEvent(event: PageEvent): void {
    this.productService.getPaginatedListOfProductDto(new PaginationDetails(
      event ? event.pageIndex : this.paginator.pageIndex,
      event ? event.pageSize : this.paginator.pageSize),
      new SortingDetails(this.sort?.active, SortingDetails.DirectionFromString(this.sort?.direction)),
      [new FilterDetails(this.filterQuery(), 'productId'),
        new FilterDetails(this.filterQuery(), 'group.name'),
        new FilterDetails(this.filterQuery(), 'size.name'),
        new FilterDetails(this.filterQuery(), 'netWeight')],
      (result) => {
        if (result) {
          this.paginator.length = result.totalCount;
          this.tableData = new MatTableDataSource<ProductDto>(result.items);
        }
      }
    );
  }
  //#endregion
}
