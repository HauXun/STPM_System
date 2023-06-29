import { connect } from 'react-redux';
import TopicPostList from '../components/TopicPostList';
import { AppDispatch, RootState } from '~/app/stores/store';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { topicActions } from '../../infrastructure/store/topicSlice';

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

export default connect(mapStateToProps, mapDispatchToProps)(TopicPostList);
