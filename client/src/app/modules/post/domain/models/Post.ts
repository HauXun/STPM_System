import { User } from "~/app/modules/user/domain/models/User";
import { PostPhoto } from "./PostPhoto";
import { PostVideo } from "./PostVideo";
import { Tag } from "~/app/modules/tag/domain/models/Tag";

export interface Post {
  id: number;
  title: string;
  shortDescription: string;
  description: string;
  meta: string;
  urlSlug: string;
  viewCount?: number;
  published: boolean;
  postedDate: Date;
  modifiedDate?: Date;
  userId: number;
  user: User;
  tags: Tag[];
  postPhotos: PostPhoto[];
  postVideos: PostVideo[];
}
