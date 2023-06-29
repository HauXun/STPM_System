import { PayloadAction } from "@reduxjs/toolkit";
import { call, put, takeLatest, debounce } from "redux-saga/effects";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { defaultPostService } from "~/app/modules/shared/common";
import { Post } from "../../domain/models/Post";
import { PostFilterModel } from "../../domain/models/PostFilterModel";
import { PostService } from "../../domain/services/PostServices";
import { postActions } from "../../infrastructure/store/postSlice";

export function createDefaultPostService() {
  return defaultPostService;
}

export function* getPosts(action: PayloadAction<PostFilterModel>) {
  try {
    const defaultPostService: PostService = yield call(createDefaultPostService);
    const result: PaginationResponse<Post[]> = yield defaultPostService.getPosts(action.payload);
    yield put(postActions.fetchPostsSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(postActions.fetchPostsError(error.message));
    } else {
      yield put(postActions.fetchPostsError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<PostFilterModel>) {
  yield put(postActions.setFilter(action.payload));
}

export default function* postSaga() {
  yield takeLatest(postActions.fetchPosts, getPosts);

  yield debounce(500, postActions.setFilterWithDebounce.type, handleSearchDebounce);
}
