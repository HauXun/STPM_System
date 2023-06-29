import { PaginationResponse } from "~/app/modules/config/api/api";
import { CommentFilterModel } from "../models/CommentFilterModel";
import { Comment } from "../models/Comment";
import { CommentRepository } from "../repositories/CommentRepository";

export class CommentService {
  constructor(private readonly topicRepository: CommentRepository) {}

  async getComments(model: CommentFilterModel): Promise<PaginationResponse<Comment[]>> {
    return await this.topicRepository.getComments(model);
  }

  async getCommentsById(id: string): Promise<Comment> {
    return this.topicRepository.getCommentsById(id);
  }
}