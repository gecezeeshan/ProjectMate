
import React, { useState } from 'react';
import api from '../api/axios';
import { useNavigate, Link } from 'react-router-dom';

const Register: React.FC = () => {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    try {
      const { data } = await api.post('/auth/register', { email, name, password });
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify({ email: data.email, name: data.name }));
      navigate('/');
    } catch (err: any) {
      setError(err?.response?.data || 'Registration failed');
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-md-4">
        <h3>Register</h3>
        {error && <div className="alert alert-danger">{error}</div>}
        <form onSubmit={submit}>
          <div className="mb-3">
            <label className="form-label">Name</label>
            <input className="form-control" value={name} onChange={e=>setName(e.target.value)} required minLength={2}/>
          </div>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input className="form-control" type="email" value={email} onChange={e=>setEmail(e.target.value)} required/>
          </div>
          <div className="mb-3">
            <label className="form-label">Password</label>
            <input className="form-control" type="password" value={password} onChange={e=>setPassword(e.target.value)} required minLength={6}/>
          </div>
          <button className="btn btn-primary w-100" type="submit">Create Account</button>
        </form>
        <div className="mt-3">
          <span>Already have an account? </span><Link to="/login">Login</Link>
        </div>
      </div>
    </div>
  );
}

export default Register;
