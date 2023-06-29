import { connect } from 'react-redux';
import TopicRankDetail from '../components/TopicRankDetail';
import { AppDispatch, RootState } from '~/app/stores/store';
import { RankAward } from '~/app/modules/rankAward/domain/models/RankAward';

const mapStateToProps = (state: RootState, ownProp: { rankAwardList: RankAward[] }) => {
  return {
    rankAwardList: ownProp.rankAwardList,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => ({});

export default connect(mapStateToProps, mapDispatchToProps)(TopicRankDetail);
