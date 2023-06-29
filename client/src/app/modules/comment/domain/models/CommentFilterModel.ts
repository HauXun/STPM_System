import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface CommentFilterModel extends PagingModel {
  keyword: string;
  year?: number;
  month?: number;
  day?: number;
  userSlug: string;
  topicSlug: string;
  postSlug: string;
  userId?: number;
  topicId?: number;
  postId?: number;
  
  userList: string[];
  topicList: string[];
  postList: string[];
  monthList: string[];
}
