import { PayloadAction } from "@reduxjs/toolkit";
import { call, put, takeLatest, debounce } from "redux-saga/effects";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { defaultTagService } from "~/app/modules/shared/common";
import { Tag } from "../../domain/models/Tag";
import { TagFilterModel } from "../../domain/models/TagFilterModel";
import { TagService } from "../../domain/services/TagServices";
import { tagActions } from "../../infrastructure/store/tagSlice";

export function createDefaultTagService() {
  return defaultTagService;
}

export function* getTags(action: PayloadAction<TagFilterModel>) {
  try {
    const defaultTagService: TagService = yield call(createDefaultTagService);
    const result: PaginationResponse<Tag[]> = yield defaultTagService.getTags(action.payload);
    yield put(tagActions.fetchTagsSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(tagActions.fetchTagsError(error.message));
    } else {
      yield put(tagActions.fetchTagsError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<TagFilterModel>) {
  yield put(tagActions.setFilter(action.payload));
}

export default function* tagSaga() {
  yield takeLatest(tagActions.fetchTags, getTags);

  yield debounce(500, tagActions.setFilterWithDebounce.type, handleSearchDebounce);
}
