# JourneyTogether

1) Architecture & Decisions
The backend is currently a monolith to keep development fast, simple, and easy to manage.
The structure is already organized by domains, which can allow us later to scale into microservices if needed, without rewriting everything, making it easy to split in:
Users | Journeys | Notifications | Auditing

2) Backend Flow
Frontend → Controller → MediatR → Handler → (Service / Events) → Repository → Database
Controllers (handle requests)
MediatR (decouples logic)
Handlers (contain core behavior)
Services / Events (handle complex logic or side effects where needed)
Repositories (manage data access)

3) Frontend Choice
The frontend uses Vue 3 with JavaScript.
This choice comes from recent experience, allowing faster development and better familiarity with modern patterns like the Composition API.
It keeps things lightweight, flexible, and productive, while still building a clean and scalable UI.