import { ApiDataResponse, PaginationResponse } from '~/app/modules/config/api/api';
import { defaultApi } from '~/app/modules/shared/common/api';
import { User } from '../../domain/models/User';
import { UserFilterModel } from '../../domain/models/UserFilterModel';
import { UserTopicRating } from '../../domain/models/UserTopicRating';

export class UserDataSource {
  private url = '/users';

  async getUsers(filterModel: UserFilterModel): Promise<ApiDataResponse<PaginationResponse<User[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<User[]>>>(this.url, { params: filterModel });
  }

  async getUserTopicRating(userId: string, topicId: string): Promise<ApiDataResponse<UserTopicRating>> {
    return await defaultApi.get<ApiDataResponse<UserTopicRating>>(`${this.url}/${userId}/topicrating/${topicId}`);
  }

  async getUsersById(id: string): Promise<ApiDataResponse<User>> {
    return await defaultApi.get<ApiDataResponse<User>>(`${this.url}/${id}`);
  }

  async addOrUpdateUser(model: User): Promise<ApiDataResponse<User>> {
    return await defaultApi.post<ApiDataResponse<User>>(`${this.url}`, model);
  }

  async deleteUser(id: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${id}`);
  }
}
