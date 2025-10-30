
# Mini Project Manager — Assignments 2 & 3

This repo solves:
- **Assignment 2 – Mini Project Manager** (JWT auth, Projects, Tasks, EF Core SQLite, React)
- **Assignment 3 – Smart Scheduler API** (auto task planning endpoint + UI)

Source: see assignment PDF (uploaded) 【7†PLC Home Coding assignment Oct 2025.pdf†L69-L115】.

## Tech
- **Backend:** .NET 8 Web API, EF Core (SQLite), JWT, Swagger
- **Frontend:** React (CRA + TypeScript), Bootstrap, React Router, Axios interceptors

## Quickstart

### Backend
```bash
cd backend
dotnet restore
dotnet run --urls https://localhost:5001;http://localhost:5000
```
- SQLite file: `app.db` (auto created)
- Swagger: http://localhost:5000/swagger

> Update `appsettings.json` -> `Jwt:Key` with a long random secret in prod.

### Frontend
```bash
cd frontend
# If CRA deps missing, add react-scripts:
npm install react-scripts --save-dev
npm install
# Copy .env.example to .env if needed and adjust API base
npm start
```

## API Overview
- `POST /api/v1/auth/register` `{ email, name, password }`
- `POST /api/v1/auth/login` `{ email, password }` → `{ token }`

- `GET  /api/v1/projects`
- `POST /api/v1/projects` `{ title, description? }`
- `GET  /api/v1/projects/{id}` (with tasks)
- `DELETE /api/v1/projects/{id}`

- `POST   /api/v1/projects/{projectId}/tasks` `{ title, dueDate? }`
- `PUT    /api/v1/projects/{projectId}/tasks/{taskId}` `{ title, dueDate?, isCompleted }`
- `DELETE /api/v1/projects/{projectId}/tasks/{taskId}`

- **Smart Scheduler**
  - `POST /api/v1/projects/{projectId}/schedule` → `{ plan: [...] }` (orders pending tasks by due date)

## Notes
- Minimal but production‑style structure (DTOs, services, validation).
- CORS: for local dev, use the same origin or add a CORS policy in `Program.cs`.
- Migrations: sample uses `EnsureCreated()` for speed; you can switch to migrations for real deployments.
- Deployment: Render (backend), Vercel/Netlify (frontend). Add proper `ASPNETCORE_URLS` and `REACT_APP_API_BASE`.
"# ProjectMate" 
