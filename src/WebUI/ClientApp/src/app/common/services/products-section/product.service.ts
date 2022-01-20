import { Injectable } from "@angular/core";
import { convertToApiModelFilters } from "../../helpers/filter-details-extensions";
import { FilterDetails } from "../../models/filter-details";
import { PaginationDetails } from "../../models/pagination-details";
import { SortingDetails, SortingDirection } from "../../models/sorting-details";
import { CreateProductCommand, PaginatedListOfProductDto, ProductClient, ProductDto, UpdateProductCommand } from "../web-api-client";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private defaultPageIndex: number = 0;
  private defaultPageSize: number = 25;
  private defaultSortDirection: SortingDirection.ascending;

  constructor(private productClient: ProductClient) {

  }

  getPaginatedListOfProductDto(page: PaginationDetails, sort: SortingDetails, filters: FilterDetails[], onSuccess: (result: PaginatedListOfProductDto) => void) {
    if (!page) {
      page = new PaginationDetails(this.defaultPageIndex, this.defaultPageSize);
    }

    if (!sort) {
      sort = new SortingDetails(null, this.defaultSortDirection);
    }

    this.productClient.getWithPagination(page.index, page.size, sort.direction, sort.property, convertToApiModelFilters(filters))
      .subscribe(onSuccess, error => console.error(error));
  }

  getProductDto(id: string, onSuccess: (result: ProductDto) => void) {
    this.productClient.get(id)
      .subscribe(onSuccess, error => console.error(error));
  }

  createProduct(command: CreateProductCommand, onSuccess: (result: string) => void) {
    this.productClient.create(command)
      .subscribe(onSuccess, error => console.error(error));
  }

  updateProduct(id: string, command: UpdateProductCommand, onSuccess: () => void) {
    this.productClient.update(id, command)
      .subscribe(onSuccess, error => console.error(error));
  }

  deleteProduct(id: string, onSuccess: () => void) {
    this.productClient.delete(id)
      .subscribe(onSuccess, error => console.error(error));
  }
}
