import { DefaultState } from "~/app/stores/store";
import { Post } from "../../domain/models/Post";
import { PostFilterModel } from "../../domain/models/PostFilterModel";

export type PostState = DefaultState & {
  data: Post[];
  filter: PostFilterModel;
}