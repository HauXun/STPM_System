import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import Post from '../components/Post';

const mapStateToProps = (state: RootState, ownProps: {postId: string}) => {
  return {
    postId: ownProps.postId
  }
};

const mapDispatchToProps = (dispatch: AppDispatch) => ({})

export default connect(mapStateToProps, mapDispatchToProps)(Post);
