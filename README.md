#  URL Shortener App

A full-stack URL shortener built with **C# (.NET)**, **Angular**, **Redis**, and **PostgreSQL**, powered by **Docker** for development and deployment.

## 📦 Tech Stack

| Layer          | Tech               |
|----------------|--------------------|
| Frontend       | Angular            |
| Backend        | C# (.NET 8 Web API)|
| Database       | PostgreSQL         |
| Cache          | Redis              |
| Rate Limiting  | In-memory middleware |
| Containerization| Docker + Docker Compose |

## To run this app:
Backend (C# API)
- cd backend
- docker-compose up --build (must have docker)

Frontend (Angular)
- cd frontend
- npm install
- ng serve

TODOs / Future Improvements
 Add unit/integration tests
 Admin analytics page (top shortened URLs)
 Rate limiter backed by Redis
 Auto-expiry of old links
