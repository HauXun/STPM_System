import { PagedList } from '~/app/modules/core/domain/models/PagedList';
import { Action, ThunkAction, combineReducers, configureStore } from "@reduxjs/toolkit";
import createSagaMiddleware from "redux-saga";
import rootSaga from "./rootSaga";
import topicReducer from "../modules/topic/infrastructure/store/topicSlice";
import commentReducer from '../modules/comment/infrastructure/store/commentSlice';
import userReducer from '../modules/user/infrastructure/store/userSlice';
import postReducer from '../modules/post/infrastructure/store/postSlice';
import tagReducer from '../modules/tag/infrastructure/store/tagSlice';
import rankAwardReducer from '../modules/rankAward/infrastructure/store/rankAwardSlice';

const sagaMiddleware = createSagaMiddleware();

const rootReducer = combineReducers({
  user: userReducer,
  topic: topicReducer,
  comment: commentReducer,
  post: postReducer,
  tag: tagReducer,
  rankAward: rankAwardReducer
});

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(sagaMiddleware)
});

sagaMiddleware.run(rootSaga);

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
export type DefaultState = {
  pagination: PagedList | null;
  isLoading: boolean;
  error: string | null;
}