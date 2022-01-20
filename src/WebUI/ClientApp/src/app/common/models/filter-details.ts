import { isNullOrWhiteSpaces } from "../helpers/string-extensions";

export interface IFilterDetails {
  query: string;
  property: string;
}

export class FilterDetails implements IFilterDetails {
  public query: string;
  public property: string;

  constructor(query: string, property: string) {
    if (isNullOrWhiteSpaces(query) || isNullOrWhiteSpaces(property)) {
      this.query = null;
      this.property = null;
    }
    else {
      this.query = query;
      this.property = property;
    }
  }
}
