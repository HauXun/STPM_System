import { PaginationResponse } from '~/app/modules/config/api/api';
import { Topic } from '../../domain/models/Topic';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { TopicRepository } from '../../domain/repositories/TopicRepository';
import { TopicDataSource } from '../data-source/TopicDataSource';

export class TopicRepositoryImpl implements TopicRepository {
  constructor(private readonly dataSource: TopicDataSource) {}

  async getTopics(model: TopicFilterModel): Promise<PaginationResponse<Topic[]>> {
    const response = await this.dataSource.getTopics(model);
    return response.result;
  }

  async getTopicsById(id: number): Promise<Topic> {
    const response = await this.dataSource.getTopicsById(id);
    return response.result;
  }
}
