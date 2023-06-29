import { User } from "~/app/modules/user/domain/models/User";
import { TopicRank } from "./TopicRank";
import { Comment } from "~/app/modules/comment/domain/models/Comment";
import { TopicPhoto } from "./TopicPhoto";
import { TopicVideo } from "./TopicVideo";
import { UserTopicRating } from "~/app/modules/user/domain/models/UserTopicRating";
import { SpecificAward } from "~/app/modules/rankAward/domain/models/SpecificAward";

export interface Topic {
  id: number;
  topicName: string;
  shortDescription: string;
  description: string;
  urlSlug : string;
  outlineUrl : string;
  regisDate : Date;
  registered : boolean;
  cancel : boolean;
  cancelDate?: Date;
  forceLock : boolean;
  regisTemp : string;
  topicRankId : number;
  leaderId : number;
  specificAwardId?: number;
  specificAward: SpecificAward;
  topicRank: TopicRank;
  leader: User;
  userTopicRatings: UserTopicRating[];
  comments: Comment[];
  users: User[];
  topicPhotos: TopicPhoto[];
  topicVideos: TopicVideo[];
}
