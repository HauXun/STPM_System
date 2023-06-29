import { PaginationResponse } from "~/app/modules/config/api/api";
import { Post } from "../models/Post";
import { PostFilterModel } from "../models/PostFilterModel";
import { PostRepository } from "../repositories/PostRepository";

export class PostService {
  constructor(private readonly topicRepository: PostRepository) {}

  async getPosts(model: PostFilterModel): Promise<PaginationResponse<Post[]>> {
    return await this.topicRepository.getPosts(model);
  }

  async getRandomPosts(limit: number): Promise<Post[]> {
    return await this.topicRepository.getRandomPosts(limit);
  }
  async getPopularPosts(limit: number): Promise<Post[]> {
    return await this.topicRepository.getPopularPosts(limit);
  }

  async getPostsById(id: string): Promise<Post> {
    return this.topicRepository.getPostsById(id);
  }
}