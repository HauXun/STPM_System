import { all } from 'redux-saga/effects';
import topicSaga from '../modules/topic/use-case/sagas/topicSagas';

export default function* rootSaga() {
  yield all([topicSaga()]);
}