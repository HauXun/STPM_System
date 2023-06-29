import { PayloadAction } from "@reduxjs/toolkit";
import { call, debounce, put, takeLatest } from "redux-saga/effects";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { defaultCommentService } from "~/app/modules/shared/common";
import { Comment } from "../../domain/models/Comment";
import { CommentFilterModel } from "../../domain/models/CommentFilterModel";
import { CommentService } from "../../domain/services/CommentServices";
import { commentActions } from "../../infrastructure/store/commentSlice";

export function createDefaultCommentService() {
  return defaultCommentService;
}

export function* getComments(action: PayloadAction<CommentFilterModel>) {
  try {
    const defaultCommentService: CommentService = yield call(createDefaultCommentService);
    const result: PaginationResponse<Comment[]> = yield defaultCommentService.getComments(action.payload);
    yield put(commentActions.fetchCommentsSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(commentActions.fetchCommentsError(error.message));
    } else {
      yield put(commentActions.fetchCommentsError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<CommentFilterModel>) {
  yield put(commentActions.setFilter(action.payload));
}

export default function* commentSaga() {
  yield takeLatest(commentActions.fetchComments, getComments);

  yield debounce(500, commentActions.setFilterWithDebounce.type, handleSearchDebounce);
}
