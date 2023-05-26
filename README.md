# Todo App
This application serves as a sandbox for various patterns, libraries, tools etc, and will probably become one of the more over-engineered to-do apps

## Technologies
* .NET 6
* Web API
* Data stored in **SQL Server** and accessed using **Entity Framework Core**
* Model validation using **FluentValidation**
* Object mapping using **Automapper**
* Unit testing using **NUnit**, **Moq**, and **FluentAssertions**

## Solution overview
I try to follow Clean Architecture principles in the solution architecture. The solution architecture is loosely based off of the [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture).

* **TodoApp.Api** - contains the API endpoints.
* **TodoApp.Application** - contains the application business logic
* **TodoApp.Domain** - contains the domain model and logic - entities, exceptions
* **TodoApp.Infrastructure** - contains logic for dealing with external resources - currently just the database.

Each project within the solution has associated automated tests.

## Development workflow
To keep things simple this repo loosely follows [Trunk-based development](https://trunkbaseddevelopment.com/). Small changes in the main branch are fine if they do not break the build. Larger changes and external contributions should be in a feature branch off of the main branch. External contributions are merged in via a pull request.

## Roadmap
* Add CI using **Azure DevOps**
* Add a front end - maybe Blazor?
* Gradually expand the application to include more functionality. *TODO: create a backlog*
* CQRS/Mediatr
* Identity
* .NET 7
