import { useState } from 'react';

const emptyForm = {
  username: '',
  password: '',
};

const AuthPage = ({
  mode,
  onToggleMode,
  onSubmit,
  loading = false,
  error = '',
  info = '',
}) => {
  const [formData, setFormData] = useState(emptyForm);
  const isLogin = mode === 'login';

  const handleSubmit = async (event) => {
    event.preventDefault();
    await onSubmit(formData);
  };

  return (
    <main className="auth-shell">
      <section className="auth-layout">
        <div className="auth-marketing">
          <span className="auth-eyebrow">Open iT university</span>
          <h1>Student Enrollment System</h1>
        </div>

        <form className="auth-card" onSubmit={handleSubmit}>
          <div className="auth-card__header">
            <span className="auth-chip">{isLogin ? 'Login' : 'Register'}</span>
            <h2>{isLogin ? 'Welcome back' : 'Create your account'}</h2>
            <p>{isLogin ? 'Sign in to manage students and related records.' : 'Create a new account to unlock edit access.'}</p>
          </div>

          {error && <div className="auth-alert auth-alert--error">{error}</div>}
          {info && <div className="auth-alert auth-alert--info">{info}</div>}

          <label className="auth-field">
            <span>Username</span>
            <input
              type="text"
              autoComplete="username"
              value={formData.username}
              onChange={(event) => setFormData((current) => ({ ...current, username: event.target.value }))}
              placeholder="Enter username"
              required
            />
          </label>

          <label className="auth-field">
            <span>Password</span>
            <input
              type="password"
              autoComplete={isLogin ? 'current-password' : 'new-password'}
              value={formData.password}
              onChange={(event) => setFormData((current) => ({ ...current, password: event.target.value }))}
              placeholder="Enter password"
              required
            />
          </label>

          <button className="auth-submit" type="submit" disabled={loading}>
            {loading ? 'Working...' : isLogin ? 'Sign in' : 'Register'}
          </button>

          <button className="auth-switch" type="button" onClick={onToggleMode}>
            {isLogin ? 'Need an account? Register' : 'Have an account? Sign in'}
          </button>
        </form>
      </section>
    </main>
  );
};

export default AuthPage;
