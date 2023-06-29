import { PaginationResponse } from '~/app/modules/config/api/api';
import { User } from '../../domain/models/User';
import { UserFilterModel } from '../../domain/models/UserFilterModel';
import { UserRepository } from '../../domain/repositories/UserRepository';
import { UserDataSource } from '../data-source/UserDataSource';
import { UserTopicRating } from '../../domain/models/UserTopicRating';

export class UserRepositoryImpl implements UserRepository {
  constructor(private readonly dataSource: UserDataSource) {}

  async getUsers(model: UserFilterModel): Promise<PaginationResponse<User[]>> {
    const response = await this.dataSource.getUsers(model);
    return response.result;
  }

  async getUserTopicRating(userId: string, topicId: string): Promise<UserTopicRating> {
    const response = await this.dataSource.getUserTopicRating(userId, topicId);
    return response.result;
  }

  async getUsersById(id: string): Promise<User> {
    const response = await this.dataSource.getUsersById(id);
    return response.result;
  }
}
