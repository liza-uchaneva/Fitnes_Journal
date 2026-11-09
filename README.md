# Fitness Journal

A full-stack fitness tracking app: sign up, set your goals during onboarding, and log workouts on a calendar.

## Features

- Sign up, log in and password recovery screens
- Onboarding flow for setting fitness goals
- Home page with daily view and goal setup
- Calendar page
- REST API for users, profiles and workouts

## Tech stack

**Frontend** (`Fitness_Journal/ClientApp`): Vue 3, Vue Router, SCSS (variables, mixins, per-page partials), Vite

**Backend** (`Fitness_Journal`): ASP.NET Core (.NET 8) Web API, Entity Framework Core + SQL Server, ASP.NET Core Identity, Swagger

## Project structure

```
Fitness_Journal/
  Controllers/    UsersController, ProfilesController
  Data/           ApplicationDbContext, User, Profile, Workout entities
  Migrations/     EF Core migrations
  ClientApp/      Vue 3 single-page app
```

## Running locally

```bash
# API: set the connection string in appsettings.Development.json, then
cd Fitness_Journal
dotnet ef database update
dotnet run

# Frontend
cd Fitness_Journal/ClientApp
npm install
npm run dev
```

## Status

Work in progress.
