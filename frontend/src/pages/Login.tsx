
import React, { useState } from 'react';
import api from '../api/axios';
import { useNavigate, Link } from 'react-router-dom';

const Login: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    try {
      const { data } = await api.post('/auth/login', { email, password });
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify({ email: data.email, name: data.name }));
      navigate('/');
    } catch (err: any) {
      setError(err?.response?.data || 'Login failed');
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-md-4">
        <h3>Login</h3>
        {error && <div className="alert alert-danger">{error}</div>}
        <form onSubmit={submit}>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input className="form-control" type="email" value={email} onChange={e=>setEmail(e.target.value)} required/>
          </div>
          <div className="mb-3">
            <label className="form-label">Password</label>
            <input className="form-control" type="password" value={password} onChange={e=>setPassword(e.target.value)} required/>
          </div>
          <button className="btn btn-primary w-100" type="submit">Login</button>
        </form>
        <div className="mt-3">
          <span>No account? </span><Link to="/register">Register</Link>
        </div>
      </div>
    </div>
  );
}

export default Login;
