import { TopicRank } from "~/app/modules/topic/domain/models/TopicRank";
import { SpecificAward } from "./SpecificAward";

export interface RankAward {
  id: number;
  awardName: string;
  urlSlug: string;
  shortDescription: string;
  description: string;
  topicRankId: number;
  specificAwards: SpecificAward[];
  topicRank: TopicRank;
}
