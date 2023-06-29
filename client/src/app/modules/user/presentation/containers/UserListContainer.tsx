import { connect } from 'react-redux';
import UserList from '../components/UserList';
import { AppDispatch, RootState } from '~/app/stores/store';
import { UserFilterModel } from '../../domain/models/UserFilterModel';
import { userActions } from '../../infrastructure/store/userSlice';

const mapStateToProps = (state: RootState) => {
  return {
    userState: state.user,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchUsers: (model: UserFilterModel) => dispatch(userActions.fetchUsers(model)),
    setFilter: (model: UserFilterModel) => dispatch(userActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(UserList);
