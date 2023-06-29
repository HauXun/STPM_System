import { connect } from 'react-redux';
import TopicList from '../components/TopicList';
import { AppDispatch, RootState } from '~/app/stores/store';
import { topicActions } from '../../infrastructure/store/topicSlice';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';

const mapStateToProps = (state: RootState) => {
  return {
    topicState: state.topic,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchTopics: (model: TopicFilterModel) => dispatch(topicActions.fetchTopics(model)),
    setFilter: (model: TopicFilterModel) => dispatch(topicActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(TopicList);
