import { PaginationResponse } from "~/app/modules/config/api/api";
import { Tag } from "../models/Tag";
import { TagFilterModel } from "../models/TagFilterModel";
import { TagRepository } from "../repositories/TagRepository";

export class TagService {
  constructor(private readonly topicRepository: TagRepository) {}

  async getTags(model: TagFilterModel): Promise<PaginationResponse<Tag[]>> {
    return await this.topicRepository.getTags(model);
  }

  async getTagsById(id: string): Promise<Tag> {
    return this.topicRepository.getTagsById(id);
  }
}