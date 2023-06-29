import { TopicService } from '~/app/modules/topic/domain/services/TopicServices';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';
import { topicActions } from './../../infrastructure/store/topicSlice';
import { call, debounce, put, takeLatest } from 'redux-saga/effects';
import { PayloadAction } from '@reduxjs/toolkit';
import { PaginationResponse } from '~/app/modules/config/api/api';
import { Topic } from '../../domain/models/Topic';
import { defaultTopicService } from '~/app/modules/shared/common';

export function createDefaultTopicService() {
  return defaultTopicService;
}

export function* getTopics(action: PayloadAction<TopicFilterModel>) {
  try {
    const defaultTopicService: TopicService = yield call(createDefaultTopicService);
    const result: PaginationResponse<Topic[]> = yield defaultTopicService.getTopics(action.payload);
    yield put(topicActions.fetchTopicsSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(topicActions.fetchTopicsError(error.message));
    } else {
      yield put(topicActions.fetchTopicsError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<TopicFilterModel>) {
  yield put(topicActions.setFilter(action.payload));
}

export default function* topicSaga() {
  yield takeLatest(topicActions.fetchTopics, getTopics);

  yield debounce(500, topicActions.setFilterWithDebounce.type, handleSearchDebounce);
}
