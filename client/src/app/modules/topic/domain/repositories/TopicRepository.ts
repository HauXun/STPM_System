import { PaginationResponse } from "~/app/modules/config/api/api";
import { Topic } from "../models/Topic";
import { TopicFilterModel } from "../models/TopicFilterModel";

export interface TopicRepository {
  getTopics(model: TopicFilterModel): Promise<PaginationResponse<Topic[]>>;
  getTopicsById(id: string): Promise<Topic>;
}
