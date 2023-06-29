import { ApiDataResponse, PaginationResponse } from "~/app/modules/config/api/api";
import { defaultApi } from "~/app/modules/shared/common/api";
import { Post } from "../../domain/models/Post";
import { PostFilterModel } from "../../domain/models/PostFilterModel";

export class PostDataSource {
  private url = '/posts';

  async getPosts(filterModel: PostFilterModel): Promise<ApiDataResponse<PaginationResponse<Post[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<Post[]>>>(this.url, { params: filterModel });
  }

  async getRandomPosts(limit: number): Promise<ApiDataResponse<Post[]>> {
    return await defaultApi.get<ApiDataResponse<Post[]>>(`${this.url}/random/${limit}`);
  }

  async getPopularPosts(limit: number): Promise<ApiDataResponse<Post[]>> {
    return await defaultApi.get<ApiDataResponse<Post[]>>(`${this.url}/featured/${limit}`);
  }

  async getPostsById(id: string): Promise<ApiDataResponse<Post>> {
    return await defaultApi.get<ApiDataResponse<Post>>(`${this.url}/${id}`);
  }

  async addOrUpdatePost(model: Post): Promise<ApiDataResponse<Post>> {
    return await defaultApi.post<ApiDataResponse<Post>>(`${this.url}`, model);
  }

  async deletePost(id: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${id}`);
  }
}
