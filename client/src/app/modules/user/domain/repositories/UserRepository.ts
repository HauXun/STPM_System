import { PaginationResponse } from "~/app/modules/config/api/api";
import { User } from "../models/User";
import { UserFilterModel } from "../models/UserFilterModel";
import { UserTopicRating } from "../models/UserTopicRating";

export interface UserRepository {
  getUsers(model: UserFilterModel): Promise<PaginationResponse<User[]>>;
  getUserTopicRating(userId: string, topicId: string): Promise<UserTopicRating>;
  getUsersById(id: string): Promise<User>;
}
