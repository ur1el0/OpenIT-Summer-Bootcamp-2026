import AuthPage from './AuthPage';

const RegisterPage = (props) => (
  <AuthPage
    {...props}
    mode="register"
  />
);

export default RegisterPage;
