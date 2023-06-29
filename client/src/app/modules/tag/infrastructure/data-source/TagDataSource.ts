import { ApiDataResponse, PaginationResponse } from "~/app/modules/config/api/api";
import { defaultApi } from "~/app/modules/shared/common/api";
import { Tag } from "../../domain/models/Tag";
import { TagFilterModel } from "../../domain/models/TagFilterModel";

export class TagDataSource {
  private url = '/tags';

  async getTags(filterModel: TagFilterModel): Promise<ApiDataResponse<PaginationResponse<Tag[]>>> {
    return await defaultApi.get<ApiDataResponse<PaginationResponse<Tag[]>>>(this.url, { params: filterModel });
  }

  async getTagsById(id: string): Promise<ApiDataResponse<Tag>> {
    return await defaultApi.get<ApiDataResponse<Tag>>(`${this.url}/${id}`);
  }

  async addOrUpdateTag(model: Tag): Promise<ApiDataResponse<Tag>> {
    return await defaultApi.post<ApiDataResponse<Tag>>(`${this.url}`, model);
  }

  async deleteTag(id: string): Promise<void> {
    await defaultApi.delete(`${this.url}/${id}`);
  }
}
