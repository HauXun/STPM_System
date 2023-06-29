import { connect } from 'react-redux';
import { RootState } from '~/app/stores/store';
import UserSignUpForm from '../components/UserSignUpForm';

const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(UserSignUpForm);
