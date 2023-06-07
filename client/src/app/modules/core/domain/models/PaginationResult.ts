import { PagingMetadata } from './PagingMetadata';

export interface PaginationResult<T> {
  items: T[];
  metadata: PagingMetadata;
}
