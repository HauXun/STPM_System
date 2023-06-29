import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface TagFilterModel extends PagingModel {
  keyword: string;
  name: string;
  postSlug: string;
  postId?: number;
}
