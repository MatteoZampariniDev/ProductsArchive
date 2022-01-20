export interface IPaginationDetails {
  index: number;
  size: number;
}

export class PaginationDetails implements IPaginationDetails {
  public index: number;
  public size: number;

  constructor(index: number, size: number) {
    if (!index || index < 0) {
      index = 0;
    }

    if (!size || size < 1) {
      size = 30;
    }

    this.index = index;
    this.size = size;
  }
}
