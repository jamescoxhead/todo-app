# Todo App
[![build](https://github.com/jamescoxhead/todo-app/actions/workflows/build.yaml/badge.svg)](https://github.com/jamescoxhead/todo-app/actions/workflows/build.yaml)

This application serves as a sandbox for various patterns, libraries, tools etc, and will probably become one of the more over-engineered to-do apps

## Technologies
* .NET 7
* Web API
* Data stored in **SQL Server** and accessed using **Entity Framework Core**
* Model validation using **FluentValidation**
* Object mapping using **Automapper**
* Unit testing using **NUnit**, **NSubstitute**, and **FluentAssertions**

## Solution overview
I try to follow Clean Architecture principles in the solution architecture. The solution architecture is loosely based off of the [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture).

* **TodoApp.Api** - contains the API endpoints.
* **TodoApp.Application** - contains the application business logic
* **TodoApp.Domain** - contains the domain model and logic - entities, exceptions
* **TodoApp.Infrastructure** - contains logic for dealing with external resources - currently just the database.

Each project within the solution has associated automated tests.

## Development workflow
To keep things simple this repo loosely follows [Trunk-based development](https://trunkbaseddevelopment.com/). Small changes in the main branch are fine if they do not break the build. Larger changes and external contributions should be in a feature branch off of the main branch. External contributions are merged in via a pull request.

Commit messages follow [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) guidelines. Commit messages should be prefixed with one of the following:

* ⭐ feat(feature-name) - a new feature
* 🔨 fix - a bugfix
* 🥱 chore - updates to the repo
* 🏗️ ci - updates to CI/CD pipelines
* 📄 docs - documentation updates
* 🔁 refactor - updates and improvements to existing code
* 🖌️ style - formatting updates
* 🧪 test - changes to automated tests

## Roadmap
* Add a front end - maybe Blazor?
* Gradually expand the application to include more functionality. *TODO: create a backlog*
* Deployments. Application and infrastructure (Bicep)
