import { PagingModel } from "~/app/modules/core/domain/models/PagingModel";

export interface TopicFilterModel extends PagingModel {
  keyword: string;
  topicName: string;
  urlSlug: string;
  registered?: boolean;
  cancel?: boolean;
  forceLock?: boolean;
  year?: number;
  month?: number;
  cancelYear?: number;
  cancelMonth?: number;
  rankAwardSlug: string;
  userSlug: string;
  userId?: number;
  leaderId?: number;
  rankAwardId?: number;
  topicRankId?: number;
  mark?: number;
  
  userList: string[];
  rankAwardList: string[];
  monthList: string[];
}
