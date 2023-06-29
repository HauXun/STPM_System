import { PaginationResponse } from "~/app/modules/config/api/api";
import { CommentFilterModel } from "../../domain/models/CommentFilterModel";
import { Comment } from "../../domain/models/Comment";
import { CommentRepository } from "../../domain/repositories/CommentRepository";
import { CommentDataSource } from "../data-source/CommentDataSource";

export class CommentRepositoryImpl implements CommentRepository {
  constructor(private readonly dataSource: CommentDataSource) {}

  async getComments(model: CommentFilterModel): Promise<PaginationResponse<Comment[]>> {
    const response = await this.dataSource.getComments(model);
    return response.result;
  }

  async getCommentsById(id: string): Promise<Comment> {
    const response = await this.dataSource.getCommentsById(id);
    return response.result;
  }
}
