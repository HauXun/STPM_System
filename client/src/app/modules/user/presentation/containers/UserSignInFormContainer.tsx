import { connect } from 'react-redux';
import { RootState } from '~/app/stores/store';
import UserSignInForm from '../components/UserSignInForm';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(UserSignInForm);
