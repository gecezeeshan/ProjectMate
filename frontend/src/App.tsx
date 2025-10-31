
import React from 'react';
import { Routes, Route, Navigate, Link, useNavigate } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import ProjectDetails from './pages/ProjectDetails';

const App: React.FC = () => {
  const token = localStorage.getItem('token');
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    navigate('/login');
  };

  return (
    <div>
      <nav className="navbar navbar-expand navbar-light bg-light px-3">
        <Link to="/" className="navbar-brand">Mini PM</Link>
        <div className="navbar-nav">
          {token ? (
            <>
              <Link to="/" className="nav-link">Dashboard</Link>
              <button className="btn btn-link nav-link" onClick={logout}>Logout</button>
            </>
          ) : (
            <>
              <Link to="/login" className="nav-link">Login</Link>
              <Link to="/register" className="nav-link">Register</Link>
            </>
          )}
        </div>
      </nav>
      <div className="container py-3">
        <Routes>
          <Route path="/" element={token ? <Dashboard/> : <Navigate to="/login" />} />
          <Route path="/projects/:id" element={token ? <ProjectDetails/> : <Navigate to="/login" />} />
          <Route path="/login" element={<Login/>} />
          <Route path="/register" element={<Register/>} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
