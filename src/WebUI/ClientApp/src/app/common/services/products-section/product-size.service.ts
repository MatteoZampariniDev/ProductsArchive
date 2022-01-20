import { Injectable } from "@angular/core";
import { convertToApiModelFilters } from "../../helpers/filter-details-extensions";
import { FilterDetails } from "../../models/filter-details";
import { PaginationDetails } from "../../models/pagination-details";
import { SortingDetails, SortingDirection } from "../../models/sorting-details";
import { CreateProductSizeCommand, PaginatedListOfProductSizeDto, ProductSizeClient, ProductSizeDto, UpdateProductSizeCommand } from "../web-api-client";

@Injectable({
  providedIn: 'root'
})
export class ProductSizeService {
  private defaultPageIndex: number = 0;
  private defaultPageSize: number = 25;
  private defaultSortDirection: SortingDirection.ascending;

  constructor(private productClient: ProductSizeClient) {

  }

  getPaginatedListOfProductSizeDto(page: PaginationDetails, sort: SortingDetails, filters: FilterDetails[], onSuccess: (result: PaginatedListOfProductSizeDto) => void) {

    if (!page) {
      page = new PaginationDetails(this.defaultPageIndex, this.defaultPageSize);
    }

    if (!sort) {
      sort = new SortingDetails(null, this.defaultSortDirection);
    }

    this.productClient.getWithPagination(page.index, page.size, sort.direction, sort.property, convertToApiModelFilters(filters))
      .subscribe(onSuccess, error => console.error(error));
  }

  getProductSizeDto(id: string, onSuccess: (result: ProductSizeDto) => void) {
    this.productClient.get(id)
      .subscribe(onSuccess, error => console.error(error));
  }

  createProductSize(command: CreateProductSizeCommand, onSuccess: (result: string) => void) {
    this.productClient.create(command)
      .subscribe(onSuccess, error => console.error(error));
  }

  updateProductSize(id: string, command: UpdateProductSizeCommand, onSuccess: () => void) {
    this.productClient.update(id, command)
      .subscribe(onSuccess, error => console.error(error));
  }

  deleteProductSize(id: string, onSuccess: () => void) {
    this.productClient.delete(id)
      .subscribe(onSuccess, error => console.error(error));
  }
}
