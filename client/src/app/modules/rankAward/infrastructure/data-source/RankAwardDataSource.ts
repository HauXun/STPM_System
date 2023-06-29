import { ApiDataResponse, PaginationResponse } from "~/app/modules/config/api/api";
import { defaultApi } from "~/app/modules/shared/common/api";
import { RankAward } from "../../domain/models/RankAward";
import { RankAwardFilterModel } from "../../domain/models/RankAwardFilterModel";

export class RankAwardDataSource {
  private url = '/rankaward';

  async getRankAwards(filterModel: RankAwardFilterModel): Promise<ApiDataResponse<PaginationResponse<RankAward[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<RankAward[]>>>(this.url, { params: filterModel });
  }

  async getRankAwardsById(id: string): Promise<ApiDataResponse<RankAward>> {
    return await defaultApi.get<ApiDataResponse<RankAward>>(`${this.url}/${id}`);
  }

  async addOrUpdateRankAward(model: RankAward): Promise<ApiDataResponse<RankAward>> {
    return await defaultApi.post<ApiDataResponse<RankAward>>(`${this.url}`, model);
  }

  async deleteRankAward(id: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${id}`);
  }
}
