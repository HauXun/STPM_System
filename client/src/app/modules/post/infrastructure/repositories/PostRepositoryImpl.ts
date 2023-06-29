import { PaginationResponse } from "~/app/modules/config/api/api";
import { Post } from "../../domain/models/Post";
import { PostFilterModel } from "../../domain/models/PostFilterModel";
import { PostRepository } from "../../domain/repositories/PostRepository";
import { PostDataSource } from "../data-source/PostDataSource";

export class PostRepositoryImpl implements PostRepository {
  constructor(private readonly dataSource: PostDataSource) {}

  async getPosts(model: PostFilterModel): Promise<PaginationResponse<Post[]>> {
    const response = await this.dataSource.getPosts(model);
    return response.result;
  }

  async getPostsById(id: string): Promise<Post> {
    const response = await this.dataSource.getPostsById(id);
    return response.result;
  }

  async getRandomPosts(limit: number): Promise<Post[]> {
    const response = await this.dataSource.getRandomPosts(limit);
    return response.result;
  }
  async getPopularPosts(limit: number): Promise<Post[]> {
    const response = await this.dataSource.getPopularPosts(limit);
    return response.result;
  }
}
