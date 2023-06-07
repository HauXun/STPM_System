export interface PagingMetadata {
  pageIndex: number;
  pageSize: number;
  pageCount: number;
  hasPreviousPage: boolean;
  firstItemIndex: number;
  lastItemIndex: number;
  isFirstPage: boolean;
}
