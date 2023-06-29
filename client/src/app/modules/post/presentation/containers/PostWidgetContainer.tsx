import { connect } from 'react-redux';
import { AppDispatch, RootState } from '~/app/stores/store';
import PostWidget from '../components/PostWidget';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = (dispatch: AppDispatch) => ({})

export default connect(mapStateToProps, mapDispatchToProps)(PostWidget);
