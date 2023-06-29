import { connect } from 'react-redux';
import PostList from '../components/PostList';
import { AppDispatch, RootState } from '~/app/stores/store';
import { PostFilterModel } from '../../domain/models/PostFilterModel';
import { postActions } from '../../infrastructure/store/postSlice';

const mapStateToProps = (state: RootState) => {
  return {
    postState: state.post,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchPosts: (model: PostFilterModel) => dispatch(postActions.fetchPosts(model)),
    setFilter: (model: PostFilterModel) => dispatch(postActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(PostList);
