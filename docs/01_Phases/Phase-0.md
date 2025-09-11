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

## Steps
1) **Fork this repository to your GitHub account.**
	- Make sure you are logged in to your GitHub account.
	- Search for the repository: `dotnet-bootcamp-2025/Module2_LibraryManagementSystem`
	- Find the `Fork` button on the top-right corner of the page and click it.
	
2) **Clone it to your local machine**
	- Open Visual Studio/ Rider or a terminal
	- Navigate to your workspace (suggested: `C:\Dev\BootCamp_Module_2`) 
	- From Terminal, type the following Git Command to Clone the repo: `git clone https://github.com/dotnet-bootcamp-2025/MyTodo`
	- From Visual Studio, select `Clone a repository` and paste the URL above
	- From Rider, select `Clone Repository` and paste the URL above
	
3) **Open the folder in your favorite editor (VS 2022 / Rider / VS Code)**
	- From Visual Studio, select `Open a project or solution` and select the folder you just cloned.

4) Create a new solution file named `LibraryApp.sln` in the root folder.
	- From Visual Studio, select `File > New > Project...` and then select `Blank Solution`. Name it `LibraryApp` and save it in the root folder.
	- Then from the Solution Explorer, right-click on the solution and select `Add > New Project...` to create the Console Application in the next step.


5) Create a new Console Application project named `LibraryApp.Console` in the root folder.
6) Add the project to the solution.
7) Make sure the project builds successfully.
8) Commit & push your changes.
