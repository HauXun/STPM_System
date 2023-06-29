import { PayloadAction } from "@reduxjs/toolkit";
import { call, put, takeLatest, debounce } from "redux-saga/effects";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { defaultUserService } from "~/app/modules/shared/common";
import { User } from "../../domain/models/User";
import { UserFilterModel } from "../../domain/models/UserFilterModel";
import { UserService } from "../../domain/services/UserServices";
import { userActions } from "../../infrastructure/store/userSlice";

export function createDefaultUserService() {
  return defaultUserService;
}

export function* getUsers(action: PayloadAction<UserFilterModel>) {
  try {
    const defaultUserService: UserService = yield call(createDefaultUserService);
    const result: PaginationResponse<User[]> = yield defaultUserService.getUsers(action.payload);
    yield put(userActions.fetchUsersSuccess(result));
  } catch (error: unknown) {
    // Use 'unknown' type annotation for the catch clause
    if (error instanceof Error) {
      yield put(userActions.fetchUsersError(error.message));
    } else {
      yield put(userActions.fetchUsersError('An unknown error occurred.'));
    }
  }
}

function* handleSearchDebounce(action: PayloadAction<UserFilterModel>) {
  yield put(userActions.setFilter(action.payload));
}

export default function* userSaga() {
  yield takeLatest(userActions.fetchUsers, getUsers);

  yield debounce(500, userActions.setFilterWithDebounce.type, handleSearchDebounce);
}
