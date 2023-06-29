import { PaginationResponse } from '~/app/modules/config/api/api';
import { Tag } from '../models/Tag';
import { TagFilterModel } from '../models/TagFilterModel';

export interface TagRepository {
  getTags(model: TagFilterModel): Promise<PaginationResponse<Tag[]>>;
  getTagsById(id: string): Promise<Tag>;
}
