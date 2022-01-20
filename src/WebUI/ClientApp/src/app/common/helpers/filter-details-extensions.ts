import { IFilterDetails } from "../models/filter-details";
import { FilterDetails } from "../services/web-api-client";

export function convertToApiModelFilters(filters: IFilterDetails[]): FilterDetails[] {

  if (!filters) {
    filters = [];
  }

  var tempFilters: FilterDetails[] = [];

  for (var i = 0; i < filters.length; i++) {

    tempFilters.push(<FilterDetails>
      {
        property: filters[i].property,
        query: filters[i].query
      });
  }

  return tempFilters;
}
