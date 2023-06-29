import { ApiDataResponse, PaginationResponse } from "~/app/modules/config/api/api";
import { defaultApi } from "~/app/modules/shared/common/api";
import { CommentFilterModel } from "../../domain/models/CommentFilterModel";
import { Comment } from "../../domain/models/Comment";

export class CommentDataSource {
  private url = '/comments';

  async getComments(filterModel: CommentFilterModel): Promise<ApiDataResponse<PaginationResponse<Comment[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<Comment[]>>>(this.url, { params: filterModel });
  }

  async getCommentsById(id: string): Promise<ApiDataResponse<Comment>> {
    return await defaultApi.get<ApiDataResponse<Comment>>(`${this.url}/${id}`);
  }

  async addOrUpdateComment(model: Comment): Promise<ApiDataResponse<Comment>> {
    return await defaultApi.post<ApiDataResponse<Comment>>(`${this.url}`, model);
  }

  async deleteComment(id: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${id}`);
  }
}
