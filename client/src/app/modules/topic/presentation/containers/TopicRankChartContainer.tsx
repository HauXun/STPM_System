import { connect } from 'react-redux';
import TopicRankChart from '../components/TopicRankChart';
import { RootState } from '~/app/stores/store';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(TopicRankChart);
