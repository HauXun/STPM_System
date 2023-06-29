import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import PostSimilar from '../components/PostSimilar';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = (dispatch: AppDispatch) => ({})

export default connect(mapStateToProps, mapDispatchToProps)(PostSimilar);
