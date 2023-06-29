import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import TopicPost from '../components/TopicPost';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = (dispatch: AppDispatch) => ({})

export default connect(mapStateToProps, mapDispatchToProps)(TopicPost);
