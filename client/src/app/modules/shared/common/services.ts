import { TopicService } from "~/app/modules/topic/domain/services/TopicServices";
import { TopicDataSource } from "~/app/modules/topic/infrastructure/data-source/TopicDataSource";
import { TopicRepositoryImpl } from "~/app/modules/topic/infrastructure/repositories/TopicRepositoryImpl";

export const defaultTopicService = new TopicService(new TopicRepositoryImpl(new TopicDataSource()));