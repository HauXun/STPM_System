import { connect } from 'react-redux';
import UserAchievement from '../components/UserAchievement';
import { AppDispatch, RootState } from '~/app/stores/store';
import { RankAwardFilterModel } from '~/app/modules/rankAward/domain/models/RankAwardFilterModel';
import { rankAwardActions } from '~/app/modules/rankAward/infrastructure/store/rankAwardSlice';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';
import { topicActions } from '~/app/modules/topic/infrastructure/store/topicSlice';

const mapStateToProps = (state: RootState, ownProps: { userId: string }) => ({
  userId: ownProps.userId,
  rankAwardState: state.rankAward,
  topicState: state.topic,
});


const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchRankAwards: (model: RankAwardFilterModel) => dispatch(rankAwardActions.fetchRankAwards(model)),
    setRankAwardFilter: (model: RankAwardFilterModel) => dispatch(rankAwardActions.setFilter(model)),
    fetchTopics: (model: TopicFilterModel) => dispatch(topicActions.fetchTopics(model)),
    setTopicFilter: (model: TopicFilterModel) => dispatch(topicActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(UserAchievement);
