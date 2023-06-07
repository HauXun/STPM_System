import { User } from "~/app/modules/user/domain/models/User";
import { TopicRank } from "./TopicRank";
import { SpecificAward } from "./SpecificAward";
import { Comment } from "~/app/modules/role copy/domain/services/CommentServices";
import { TopicPhoto } from "./TopicPhoto";
import { TopicVideo } from "./TopicVideo";

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
  userTopicRatings: User[];
  comments: Comment[];
  users: User[];
  topicPhotos: TopicPhoto[];
  topicVideos: TopicVideo[];
}
