import { ApiDataResponse } from "~/app/modules/config/api/api";
import { Topic } from "~/app/modules/topic/domain/models/Topic";

export interface GetTopicsResponse extends ApiDataResponse<Topic[]> {}

export interface GetTopicResponse extends ApiDataResponse<Topic> {}