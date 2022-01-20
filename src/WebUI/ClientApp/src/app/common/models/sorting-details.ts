import { isNullOrWhiteSpaces } from "../helpers/string-extensions";

export interface ISortingDetails {
  direction: string;
  property: string;
}

export class SortingDetails implements ISortingDetails {
  public direction: string;
  public property: string;

  constructor(property: string, direction: SortingDirection = SortingDirection.ascending) {
    if (isNullOrWhiteSpaces(property)) {
      property = null;
    }

    this.direction = direction;
    this.property = property;
  }

  public static DirectionFromString(direction: string): SortingDirection {
    return direction.toLowerCase() == 'desc' ? SortingDirection.descending : SortingDirection.ascending;
  }
}

export enum SortingDirection {
  ascending = "asc",
  descending = "desc"
}
