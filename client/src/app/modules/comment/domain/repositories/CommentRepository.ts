import { PaginationResponse } from '~/app/modules/config/api/api';
import { CommentFilterModel } from '../models/CommentFilterModel';
import { Comment } from '../models/Comment';

export interface CommentRepository {
  getComments(model: CommentFilterModel): Promise<PaginationResponse<Comment[]>>;
  getCommentsById(id: string): Promise<Comment>;
}
