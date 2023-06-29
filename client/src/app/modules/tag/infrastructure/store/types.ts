import { DefaultState } from "~/app/stores/store";
import { Tag } from "../../domain/models/Tag";
import { TagFilterModel } from "../../domain/models/TagFilterModel";

export type TagState = DefaultState & {
  data: Tag[];
  filter: TagFilterModel;
}