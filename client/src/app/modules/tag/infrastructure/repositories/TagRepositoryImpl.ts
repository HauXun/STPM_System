import { PaginationResponse } from "~/app/modules/config/api/api";
import { Tag } from "../../domain/models/Tag";
import { TagFilterModel } from "../../domain/models/TagFilterModel";
import { TagRepository } from "../../domain/repositories/TagRepository";
import { TagDataSource } from "../data-source/TagDataSource";

export class TagRepositoryImpl implements TagRepository {
  constructor(private readonly dataSource: TagDataSource) {}

  async getTags(model: TagFilterModel): Promise<PaginationResponse<Tag[]>> {
    const response = await this.dataSource.getTags(model);
    return response.result;
  }

  async getTagsById(id: string): Promise<Tag> {
    const response = await this.dataSource.getTagsById(id);
    return response.result;
  }
}
