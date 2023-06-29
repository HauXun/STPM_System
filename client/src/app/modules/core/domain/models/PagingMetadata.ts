export interface PagingMetadata {
  pageIndex: number;
  pageSize: number;
  totalItemCount: number;
  pageNumber: number;
  pageCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  firstItemIndex: number;
  lastItemIndex: number;
  isFirstPage: boolean;
  isLastPage: boolean;
}
