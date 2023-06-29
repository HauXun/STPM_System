import { connect } from 'react-redux';
import { RootState } from '~/app/stores/store';
import TopicSignUpForm from '../components/TopicSignUpForm';
const mapStateToProps = (state: RootState) => ({});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(TopicSignUpForm);
