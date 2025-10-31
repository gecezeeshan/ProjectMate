
import React, { useEffect, useState } from 'react';
import api from '../api/axios';
import { Link } from 'react-router-dom';

type Project = {
  id: string;
  title: string;
  description?: string;
  createdAt: string;
}

const Dashboard: React.FC = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await api.get('/projects');
      setProjects(data);
    } catch (err: any) {
      setError('Failed to load projects');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const create = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    try {
      await api.post('/projects', { title, description });
      setTitle(''); setDescription('');
      await load();
    } catch (err: any) {
      setError(err?.response?.data || 'Failed to create project');
    }
  };

  const remove = async (id: string) => {
    if (!window.confirm('Delete project?')) return;
    await api.delete(`/projects/${id}`);
    await load();
  };

  return (
    <div>
      <h3>Projects</h3>
      {error && <div className="alert alert-danger">{error}</div>}
      <form className="row g-2 mb-3" onSubmit={create}>
        <div className="col-md-4">
          <input className="form-control" placeholder="Title" value={title} onChange={e=>setTitle(e.target.value)} required minLength={3} />
        </div>
        <div className="col-md-5">
          <input className="form-control" placeholder="Description (optional)" value={description} onChange={e=>setDescription(e.target.value)} />
        </div>
        <div className="col-md-3">
          <button className="btn btn-primary w-100" type="submit">Add Project</button>
        </div>
      </form>

      {loading ? <p>Loading...</p> : (
        <div className="list-group">
          {projects.map(p => (
            <div key={p.id} className="list-group-item d-flex justify-content-between align-items-center">
              <div>
                <Link to={`/projects/${p.id}`}>
                  <strong>{p.title}</strong>
                </Link>
                {p.description && <div className="text-muted small">{p.description}</div>}
              </div>
              <button className="btn btn-outline-danger btn-sm" onClick={()=>remove(p.id)}>Delete</button>
            </div>
          ))}
          {projects.length === 0 && <div className="text-muted">No projects yet.</div>}
        </div>
      )}
    </div>
  );
}

export default Dashboard;
