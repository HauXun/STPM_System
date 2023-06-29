import { connect } from 'react-redux';
import TopicInfo from '../components/TopicInfo';
import { AppDispatch, RootState } from '~/app/stores/store';
import { Topic } from '../../domain/models/Topic';

const mapStateToProps = (state: RootState, ownProps: { topic: Topic | undefined }) => {
  return {
    topic: ownProps.topic,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => ({});

export default connect(mapStateToProps, mapDispatchToProps)(TopicInfo);
