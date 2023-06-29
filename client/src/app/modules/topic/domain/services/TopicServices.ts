import { PaginationResponse } from "~/app/modules/config/api/api";
import { Topic } from "../models/Topic";
import { TopicFilterModel } from "../models/TopicFilterModel";
import { TopicRepository } from "../repositories/TopicRepository";

export class TopicService {
  constructor(private readonly topicRepository: TopicRepository) {}

  async getTopics(model: TopicFilterModel): Promise<PaginationResponse<Topic[]>> {
    return await this.topicRepository.getTopics(model);
  }

  async getTopicsById(id: string): Promise<Topic> {
    return this.topicRepository.getTopicsById(id);
  }
}