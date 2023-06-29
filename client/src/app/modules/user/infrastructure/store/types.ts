import { DefaultState } from "~/app/stores/store";
import { User } from "../../domain/models/User";
import { UserFilterModel } from "../../domain/models/UserFilterModel";

export type UserState = DefaultState & {
  data: User[];
  filter: UserFilterModel;
}