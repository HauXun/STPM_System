import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import TopicRankInfo from '../components/TopicRankInfo';

const mapStateToProps = (state: RootState, ownProp: { topicCount: number, rankName: string }) => {
  return {
    topicCount: ownProp.topicCount,
    rankName: ownProp.rankName,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => ({});

export default connect(mapStateToProps, mapDispatchToProps)(TopicRankInfo);
