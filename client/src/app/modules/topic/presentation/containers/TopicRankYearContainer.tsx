import { connect } from 'react-redux';
import TopicRankYear from '../components/TopicRankYear';
import { AppDispatch, RootState } from '~/app/stores/store';

const mapStateToProps = (state: RootState, ) => {
  return {
    rankAwardList: state.rankAward.data
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => ({});

export default connect(mapStateToProps, mapDispatchToProps)(TopicRankYear);
