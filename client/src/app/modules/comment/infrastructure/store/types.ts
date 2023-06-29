import { DefaultState } from "~/app/stores/store";
import { CommentFilterModel } from "../../domain/models/CommentFilterModel";
import { Comment } from "../../domain/models/Comment";

export type CommentState = DefaultState & {
  data: Comment[];
  filter: CommentFilterModel;
}