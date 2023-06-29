import { PaginationResponse } from "~/app/modules/config/api/api";
import { RankAward } from "../models/RankAward";
import { RankAwardFilterModel } from "../models/RankAwardFilterModel";
import { RankAwardRepository } from "../repositories/RankAwardRepository";

export class RankAwardService {
  constructor(private readonly topicRepository: RankAwardRepository) {}

  async getRankAwards(model: RankAwardFilterModel): Promise<PaginationResponse<RankAward[]>> {
    return await this.topicRepository.getRankAwards(model);
  }

  async getRankAwardsById(id: string): Promise<RankAward> {
    return this.topicRepository.getRankAwardsById(id);
  }
}