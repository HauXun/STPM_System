import { Topic } from "../../domain/models/Topic";
import { TopicFilterModel } from "../../domain/models/TopicFilterModel";
import { PagedList } from "~/app/modules/core/domain/models/PagedList";

export interface TopicState {
  data: Topic[];
  pagination: PagedList | null;
  filter: TopicFilterModel;
  isLoading: boolean;
  error: string | null;
}