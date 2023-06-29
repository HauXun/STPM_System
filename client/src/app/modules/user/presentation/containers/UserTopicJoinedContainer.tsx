import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import UserTopicJoined from '../components/UserTopicJoined';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';
import { topicActions } from '~/app/modules/topic/infrastructure/store/topicSlice';

const mapStateToProps = (state: RootState, ownProps: { userId: string }) => ({
  userId: ownProps.userId,
  topicState: state.topic
});

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchTopics: (model: TopicFilterModel) => dispatch(topicActions.fetchTopics(model)),
    setFilter: (model: TopicFilterModel) => dispatch(topicActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(UserTopicJoined);
