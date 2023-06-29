import { connect } from 'react-redux';
import CommentList from '../components/CommentList';
import { RootState, AppDispatch } from '~/app/stores/store';
import { CommentFilterModel } from '../../domain/models/CommentFilterModel';
import { commentActions } from '../../infrastructure/store/commentSlice';

const mapStateToProps = (state: RootState) => {
  return {
    commentState: state.comment,
  };
};

const mapDispatchToProps = (dispatch: AppDispatch) => {
  return {
    fetchComments: (model: CommentFilterModel) => dispatch(commentActions.fetchComments(model)),
    setFilter: (model: CommentFilterModel) => dispatch(commentActions.setFilter(model)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(CommentList);
