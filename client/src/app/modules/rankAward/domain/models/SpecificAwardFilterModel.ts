import { PagingModel } from '~/app/modules/core/domain/models/PagingModel';

export interface SpecificAwardFilterModel extends PagingModel {
  keyword: string;
  bonusPrize?: number;
  year?: number;
  passed?: boolean;
  rankAwardSlug: string;
  topicSlug: string;
  topicId?: number;
  rankAwardId?: number;
}
