import { PaginationResponse } from "~/app/modules/config/api/api";
import { Post } from "../models/Post";
import { PostFilterModel } from "../models/PostFilterModel";

export interface PostRepository {
  getPosts(model: PostFilterModel): Promise<PaginationResponse<Post[]>>;
  getRandomPosts(limit: number): Promise<Post[]>;
  getPopularPosts(limit: number): Promise<Post[]>;
  getPostsById(id: string): Promise<Post>;
}
