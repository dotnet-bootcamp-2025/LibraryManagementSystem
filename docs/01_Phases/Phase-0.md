# Phase 0 — Project Creation & Run

**Goal:** Fork the repository and create the solution and run it.

## What you’ll learn
- Solution creation
- How to run the Console project

## Prereqs
- .NET SDK installed
- Git + an editor (VS 2022 Community Edition / Rider / VS Code)

## Proposed Project Structure
```
LibraryApp.Console/			# Console UI application
   ├─ Program.cs
   ├─ Domain/				# Core domain entities
   |   ├─ LibraryItem.cs
   |   ├─ Book.cs
   |   ├─ Magazine.cs
   |   ├─ Member.cs
   ├─ Services/				# Repository implementation (in-memory)
   |   ├─ LibraryService.cs
   ├─ Utils/				# Helpers and utilities
   |   ├─ InputHelper.cs
```
