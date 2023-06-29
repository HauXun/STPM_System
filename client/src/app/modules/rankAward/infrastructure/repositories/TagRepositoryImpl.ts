import { PaginationResponse } from "~/app/modules/config/api/api";
import { RankAward } from "../../domain/models/RankAward";
import { RankAwardFilterModel } from "../../domain/models/RankAwardFilterModel";
import { RankAwardRepository } from "../../domain/repositories/RankAwardRepository";
import { RankAwardDataSource } from "../data-source/RankAwardDataSource";

export class RankAwardRepositoryImpl implements RankAwardRepository {
  constructor(private readonly dataSource: RankAwardDataSource) {}

  async getRankAwards(model: RankAwardFilterModel): Promise<PaginationResponse<RankAward[]>> {
    const response = await this.dataSource.getRankAwards(model);
    return response.result;
  }

  async getRankAwardsById(id: string): Promise<RankAward> {
    const response = await this.dataSource.getRankAwardsById(id);
    return response.result;
  }
}
