import { all } from 'redux-saga/effects';
import topicSaga from '../modules/topic/use-case/sagas/topicSagas';
import userSaga from '../modules/user/use-case/sagas/userSagas';
import commentSaga from '../modules/comment/use-case/sagas/commentSagas';
import postSaga from '../modules/post/use-case/sagas/postSagas';
import tagSaga from '../modules/tag/use-case/sagas/tagSagas';
import rankAwardSaga from '../modules/rankAward/use-case/sagas/rankAwardSagas';

export default function* rootSaga() {
  yield all([topicSaga(), userSaga(), commentSaga(), postSaga(), tagSaga(), rankAwardSaga()]);
}