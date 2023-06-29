import { Topic } from '~/app/modules/topic/domain/models/Topic';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';

export interface GetTopicRequest extends TopicFilterModel {}

export interface AddOrUpdateTopicRequest
  extends Omit<
    Topic,
    | 'regisDate'
    | 'cancelDate'
    | 'specificAward'
    | 'topicRank'
    | 'leader'
    | 'userTopicRatings'
    | 'comments'
    | 'users'
    | 'topicPhotos'
    | 'topicVideos'
  > {
  outlineFile: File;
  imageFiles: File[];
  videoFiles: File[];
  users: number[];
  userRatings: number[];
}
