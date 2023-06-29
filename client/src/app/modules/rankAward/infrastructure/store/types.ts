import { DefaultState } from "~/app/stores/store";
import { RankAward } from "../../domain/models/RankAward";
import { RankAwardFilterModel } from "../../domain/models/RankAwardFilterModel";

export type RankAwardState = DefaultState & {
  data: RankAward[];
  filter: RankAwardFilterModel;
}