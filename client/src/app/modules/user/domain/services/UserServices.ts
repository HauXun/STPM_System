import { PaginationResponse } from "~/app/modules/config/api/api";
import { User } from "../models/User";
import { UserFilterModel } from "../models/UserFilterModel";
import { UserRepository } from "../repositories/UserRepository";
import { UserTopicRating } from "../models/UserTopicRating";

export class UserService {
  constructor(private readonly topicRepository: UserRepository) {}

  async getUsers(model: UserFilterModel): Promise<PaginationResponse<User[]>> {
    return await this.topicRepository.getUsers(model);
  }
  
  async getUserTopicRating(userId: string, topicId: string): Promise<UserTopicRating> {
    return await this.topicRepository.getUserTopicRating(userId, topicId);
  }

  async getUsersById(id: string): Promise<User> {
    return this.topicRepository.getUsersById(id);
  }
}