import { connect } from 'react-redux';
import UserInfo from '../components/UserInfo';
import { AppDispatch, RootState } from '~/app/stores/store';

const mapStateToProps = (state: RootState, ownProps: { userId: string }) => ({
  userId: ownProps.userId
});

const mapDispatchToProps = (dispatch: AppDispatch) => ({});

export default connect(mapStateToProps, mapDispatchToProps)(UserInfo);
