import { PaginationResponse } from "~/app/modules/config/api/api";
import { RankAward } from "../models/RankAward";
import { RankAwardFilterModel } from "../models/RankAwardFilterModel";

export interface RankAwardRepository {
  getRankAwards(model: RankAwardFilterModel): Promise<PaginationResponse<RankAward[]>>;
  getRankAwardsById(id: string): Promise<RankAward>;
}
