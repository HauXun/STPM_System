import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface RankAwardFilterModel extends PagingModel {
  keyword: string;
  awardName: string;
  urlSlug: string;
  topicSlug: string;
  topicId?: number;
  year?: number;
}
