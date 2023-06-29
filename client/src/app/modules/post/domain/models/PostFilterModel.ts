import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface PostFilterModel extends PagingModel {
  keyword: string;
  published?: boolean;
  year?: number;
  month?: number;
  day?: number;
  postSlug: string;
  userSlug: string;
  tagSlug: string;
  userId?: number;

  userList: string[];
  monthList: string[];
}
