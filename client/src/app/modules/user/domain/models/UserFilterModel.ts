import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface UserFilterModel extends PagingModel {
  keyword: string;
  lockoutEnable?: boolean;
  year?: number;
  month?: number;
  userName: string;
  email: string;
  phoneNumber: string;
  postSlug: string;
  topicSlug: string;
  userSlug: string;
  topicId?: number;
  postId?: number;

  topicList: string[];
  postList: string[];
  monthList: string[];
}
