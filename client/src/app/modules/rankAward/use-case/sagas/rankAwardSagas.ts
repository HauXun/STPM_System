import { PayloadAction } from "@reduxjs/toolkit";
import { call, debounce, put, takeLatest } from "redux-saga/effects";
import { defaultRankAwardService } from "~/app/modules/shared/common";
import { RankAward } from "../../domain/models/RankAward";
import { RankAwardFilterModel } from "../../domain/models/RankAwardFilterModel";
import { RankAwardService } from "../../domain/services/RankAwardServices";
import { rankAwardActions } from "../../infrastructure/store/rankAwardSlice";

export function createDefaultRankAwardService() {
  return defaultRankAwardService;
}

export function* getRankAwards(action: PayloadAction<RankAwardFilterModel>) {
  try {
    const defaultRankAwardService: RankAwardService = yield call(createDefaultRankAwardService);
    const result: RankAward[] = yield defaultRankAwardService.getRankAwards(action.payload);
    yield put(rankAwardActions.fetchRankAwardsSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(rankAwardActions.fetchRankAwardsError(error.message));
    } else {
      yield put(rankAwardActions.fetchRankAwardsError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<RankAwardFilterModel>) {
  yield put(rankAwardActions.setFilter(action.payload));
}

export default function* rankAwardSaga() {
  yield takeLatest(rankAwardActions.fetchRankAwards, getRankAwards);

  yield debounce(500, rankAwardActions.setFilterWithDebounce.type, handleSearchDebounce);
}
