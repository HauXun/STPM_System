import { DefaultState } from "~/app/stores/store";
import { Topic } from "../../domain/models/Topic";
import { TopicFilterModel } from "../../domain/models/TopicFilterModel";

export type TopicState = DefaultState & {
  data: Topic[];
  filter: TopicFilterModel;
}