import { animate, state, style, transition, trigger } from "@angular/animations";
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
import { ProductGroupService } from "../../../common/services/products-section/product-group.service";
import { ProductService } from "../../../common/services/products-section/product.service";
import { ProductDto, ProductGroupDto } from "../../../common/services/web-api-client";

@Component({
  selector: 'app-product-groups-table-view',
  templateUrl: './product-groups-table-view.component.html',
  animations: [
    trigger('detailExpand',
      [state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))]
    )
  ]
})
export class ProductGroupsTableViewComponent extends TableViewBaseComponent<ProductGroupDto>
{
  constructor(protected formBuilder: FormBuilder,
    protected authService: AuthorizeService,
    protected identityService: IdentityService,
    private productGroupService: ProductGroupService,
    private productService: ProductService) {
    super(formBuilder, authService, identityService);

    this.initializeComponent(
      'Gruppi',
      'Prodotti raggruppati per nome.',
      ProductsSectionRoutes.ProductGroupEditor,
      ['ID Referenza', 'Categoria', 'Nome'],
      ['groupId', 'category.name', 'name', 'buttons']
    );
  }

  //#region HTML
  public expandedItem: ProductGroupDto | null;
  public expandedItemTableHeadersIds: string[] = ['productId', 'size.name'];
  public expandedItemTableData: MatTableDataSource<ProductDto>;


  public onFilterOrSortChanged(): void {
    this.updatePage();
  }

  public onPageChanged(event: PageEvent): void {
    this.updatePageWithEvent(event);
  }

  public onDeleteItemRequest(item: ProductGroupDto): void {
    if (item) {
      this.productGroupService.deleteProductGroup(item.id, () => this.updatePage());
    }
  }

  public onProductGroupExpanded(group: ProductGroupDto) {
    this.expandedItem = this.expandedItem === group ? null : group;

    if (this.expandedItem) {
      this.productService.getPaginatedListOfProductDto(new PaginationDetails(0, 999999),
        null, [new FilterDetails(this.expandedItem.groupId, 'group.groupId')],
        (result) => {
          if (result) {
            this.expandedItemTableData = new MatTableDataSource<ProductDto>(result.items);
          }
        }
      );
    }
  }
  //#endregion

  //#region Internal
  protected updatePageWithEvent(event: PageEvent): void {
    this.productGroupService.getPaginatedListOfProductGroupDto(new PaginationDetails(
      event ? event.pageIndex : this.paginator.pageIndex,
      event ? event.pageSize : this.paginator.pageSize),
      new SortingDetails(this.sort?.active, SortingDetails.DirectionFromString(this.sort?.direction)),
      [new FilterDetails(this.filterQuery(), 'groupId'),
        new FilterDetails(this.filterQuery(), 'category.name'),
        new FilterDetails(this.filterQuery(), 'name')],
      (result) => {
        if (result) {
          this.paginator.length = result.totalCount;
          this.tableData = new MatTableDataSource<ProductGroupDto>(result.items);
        }
      }
    );
  }
  //#endregion
}
