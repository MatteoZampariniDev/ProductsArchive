import { Injectable } from "@angular/core";
import { convertToApiModelFilters } from "../../helpers/filter-details-extensions";
import { FilterDetails } from "../../models/filter-details";
import { PaginationDetails } from "../../models/pagination-details";
import { SortingDetails, SortingDirection } from "../../models/sorting-details";
import { CreateProductCategoryCommand, PaginatedListOfProductCategoryDto, ProductCategoryClient, ProductCategoryDto, UpdateProductCategoryCommand } from "../web-api-client";

@Injectable({
  providedIn: 'root'
})
export class ProductCategoryService {
  private defaultPageIndex: number = 0;
  private defaultPageSize: number = 25;
  private defaultSortDirection: SortingDirection.ascending;

  constructor(private productGroupClient: ProductCategoryClient) {

  }

  getPaginatedListOfProductCategoryDto(page: PaginationDetails, sort: SortingDetails, filters: FilterDetails[], onSuccess: (result: PaginatedListOfProductCategoryDto) => void) {
    if (!page) {
      page = new PaginationDetails(this.defaultPageIndex, this.defaultPageSize);
    }

    if (!sort) {
      sort = new SortingDetails(null, this.defaultSortDirection);
    }

    this.productGroupClient.getWithPagination(page.index, page.size, sort.direction, sort.property, convertToApiModelFilters(filters))
      .subscribe(onSuccess, error => console.error(error));
  }

  getProductCategoryDto(id: string, onSuccess: (result: ProductCategoryDto) => void) {
    this.productGroupClient.get(id)
      .subscribe(onSuccess, error => console.error(error));
  }

  createProductCategory(command: CreateProductCategoryCommand, onSuccess: (result: string) => void) {
    this.productGroupClient.create(command)
      .subscribe(onSuccess, error => console.error(error));
  }

  updateProductCategory(id: string, command: UpdateProductCategoryCommand, onSuccess: () => void) {
    this.productGroupClient.update(id, command)
      .subscribe(onSuccess, error => console.error(error));
  }

  deleteProductCategory(id: string, onSuccess: () => void) {
    this.productGroupClient.delete(id)
      .subscribe(onSuccess, error => console.error(error));
  }
}
