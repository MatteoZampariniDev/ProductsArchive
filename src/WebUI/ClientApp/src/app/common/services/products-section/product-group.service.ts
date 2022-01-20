import { Injectable } from "@angular/core";
import { convertToApiModelFilters } from "../../helpers/filter-details-extensions";
import { FilterDetails } from "../../models/filter-details";
import { PaginationDetails } from "../../models/pagination-details";
import { SortingDetails, SortingDirection } from "../../models/sorting-details";
import { CreateProductGroupCommand, PaginatedListOfProductGroupDto, ProductGroupClient, ProductGroupDto, UpdateProductGroupCommand } from "../web-api-client";

@Injectable({
	providedIn: 'root'
})
export class ProductGroupService
{
    private defaultPageIndex: number = 0;
    private defaultPageSize: number = 25;
    private defaultSortDirection: SortingDirection.ascending;

    constructor(private productGroupClient: ProductGroupClient)
    {

    }

    getPaginatedListOfProductGroupDto(page: PaginationDetails, sort: SortingDetails, filters: FilterDetails[], onSuccess: (result: PaginatedListOfProductGroupDto) => void)
    {
		if (!page)
        {
      page = new PaginationDetails(this.defaultPageIndex, this.defaultPageSize);
        }

        if (!sort)
        {
          sort = new SortingDetails(null, this.defaultSortDirection);
        }

      this.productGroupClient.getWithPagination(page.index, page.size, sort.direction, sort.property, convertToApiModelFilters(filters))
            .subscribe(onSuccess, error => console.error(error));
    }

    getProductGroupDto(id: string, onSuccess: (result: ProductGroupDto) => void)
    {
        this.productGroupClient.get(id)
            .subscribe(onSuccess, error => console.error(error));
    }

    createProductGroup(command: CreateProductGroupCommand, onSuccess: (result: string) => void)
    {
        this.productGroupClient.create(command)
            .subscribe(onSuccess, error => console.error(error));
    }

    updateProductGroup(id: string, command: UpdateProductGroupCommand, onSuccess: () => void)
    {
        this.productGroupClient.update(id, command)
            .subscribe(onSuccess, error => console.error(error));
    }

    deleteProductGroup(id: string, onSuccess: () => void)
    {
        this.productGroupClient.delete(id)
            .subscribe(onSuccess, error => console.error(error));
    }
}
