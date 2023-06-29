import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import UserPostContribute from '../components/UserPostContribute';
import { PostFilterModel } from '~/app/modules/post/domain/models/PostFilterModel';
import { postActions } from '~/app/modules/post/infrastructure/store/postSlice';

const mapStateToProps = (state: RootState, ownProps: { userId: string }) => ({
  userId: ownProps.userId,
  postState: state.post
});

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchPosts: (model: PostFilterModel) => dispatch(postActions.fetchPosts(model)),
    setFilter: (model: PostFilterModel) => dispatch(postActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(UserPostContribute);
