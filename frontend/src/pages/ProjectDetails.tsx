
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../api/axios';

type Task = { id: string; title: string; dueDate?: string; isCompleted: boolean; projectId: string };
type Project = { id: string; title: string; description?: string; createdAt: string; tasks: Task[] };

const ProjectDetails: React.FC = () => {
  const { id } = useParams();
  const [project, setProject] = useState<Project | null>(null);
  const [title, setTitle] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [plan, setPlan] = useState<any>(null);

  const load = async () => {
    try {
      const { data } = await api.get(`/projects/${id}`);
      setProject(data);
    } catch (err) {
      setError('Failed to load project');
    }
  };

  useEffect(() => { load(); }, [id]);

  const addTask = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post(`/projects/${id}/tasks`, { title, dueDate: dueDate ? new Date(dueDate).toISOString() : null });
      setTitle(''); setDueDate('');
      await load();
    } catch (err: any) {
      setError(err?.response?.data || 'Failed to add task');
    }
  };

  const toggle = async (t: Task) => {
    await api.put(`/projects/${id}/tasks/${t.id}`, { title: t.title, dueDate: t.dueDate, isCompleted: !t.isCompleted });
    await load();
  };

  const remove = async (t: Task) => {
    if (!window.confirm('Delete task?')) return;
    await api.delete(`/projects/${id}/tasks/${t.id}`);
    await load();
  };

  const schedule = async () => {
    const { data } = await api.post(`/projects/${id}/schedule`, {});
    setPlan(data);
  };

  if (!project) return <p>Loading...</p>;

  return (
    <div>
      <h3>{project.title}</h3>
      {project.description && <div className="text-muted mb-2">{project.description}</div>}

      <form className="row g-2 mb-3" onSubmit={addTask}>
        <div className="col-md-5">
          <input className="form-control" placeholder="Task title" value={title} onChange={e=>setTitle(e.target.value)} required />
        </div>
        <div className="col-md-4">
          <input className="form-control" type="date" value={dueDate} onChange={e=>setDueDate(e.target.value)} />
        </div>
        <div className="col-md-3">
          <button className="btn btn-primary w-100" type="submit">Add Task</button>
        </div>
      </form>

      <div className="mb-3">
        <button className="btn btn-outline-secondary" onClick={schedule}>Smart Schedule</button>
      </div>

      <ul className="list-group mb-3">
        {project.tasks.map(t => (
          <li key={t.id} className="list-group-item d-flex justify-content-between align-items-center">
            <div>
              <div><strong>{t.title}</strong></div>
              <div className="small text-muted">
                Due: {t.dueDate ? new Date(t.dueDate).toLocaleDateString() : '—'}
              </div>
            </div>
            <div className="d-flex gap-2">
              <button className="btn btn-sm btn-outline-success" onClick={() => toggle(t)}>
                {t.isCompleted ? 'Mark Active' : 'Mark Done'}
              </button>
              <button className="btn btn-sm btn-outline-danger" onClick={() => remove(t)}>Delete</button>
            </div>
          </li>
        ))}
        {project.tasks.length === 0 && <li className="list-group-item">No tasks yet.</li>}
      </ul>

      {plan && (
        <div className="card">
          <div className="card-header">Suggested Plan</div>
          <ul className="list-group list-group-flush">
            {plan.plan.map((p: any, i: number) => (
              <li key={i} className="list-group-item">
                <strong>{p.title}</strong> → {new Date(p.suggestedStart).toLocaleDateString()} to {new Date(p.suggestedEnd).toLocaleDateString()}
              </li>
            ))}
            {plan.plan.length === 0 && <li className="list-group-item">No pending tasks to plan.</li>}
          </ul>
        </div>
      )}
    </div>
  );
};

export default ProjectDetails;
