import { ApiDataResponse, PaginationResponse } from '~/app/modules/config/api/api';
import { Topic } from '../../domain/models/Topic';
import { AddOrUpdateTopicRequest } from '~/app/modules/interfaces/http/requests';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { defaultApi } from '~/app/modules/shared/common/api';

export class TopicDataSource {
  private url = '/topics';

  async getTopics(filterModel: TopicFilterModel): Promise<ApiDataResponse<PaginationResponse<Topic[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<Topic[]>>>(this.url, { params: filterModel });
  }

  async getTopicsById(id: number): Promise<ApiDataResponse<Topic>> {
    return await defaultApi.get<ApiDataResponse<Topic>>(`${this.url}/${id}`);
  }

  async addOrUpdateTopic(model: AddOrUpdateTopicRequest): Promise<ApiDataResponse<Topic>> {
    return await defaultApi.post<ApiDataResponse<Topic>>(`${this.url}`, model);
  }

  async deleteTopic(topicId: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${topicId}`);
  }
}
