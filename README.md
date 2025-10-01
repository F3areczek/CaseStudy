# Case Study

An application demonstrating a case study for an admission process.

## Prerequisites
- [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) is required to build the project.

## Solution Structure
The solution contains two main projects:

### 1. `CaseStudy.UnitTest`
- A project for running and debugging **Unit Tests** focused on the functionality of `CaseStudy.WebApi`.
- Includes **18 pre-prepared tests**:
  - **8 tests** for API version 1.
  - **10 tests** for API version 2.
- Test data are automatically created in the `ProductsTestData.SeedProducts` method.

### 2. `CaseStudy.WebApi`
- A **Web API** project using [**OpenAPI definitions**](https://spec.openapis.org/) for each endpoint across multiple versions.
- Provides **two versions of the API**: v1 and v2.
- The API is available and documented via a graphical [**Swagger UI**](https://swagger.io/), where you can also switch between versions.
- The project includes **launchSettings.json** configured to open the [Swagger UI](https://swagger.io/) in a web browser on startup.
- Comes with a pre-prepared [**SQLite database**](https://sqlite.org/).
  - Database **migrations** are included to create and populate the database with default data when necessary.
- Uses [**Entity Framework Core (EFC)**](https://learn.microsoft.com/en-us/ef/core/) as the ORM.

## Getting Started
1. Clone the repository.
2. Run the `CaseStudy.WebApi` project.
3. Navigate to the [Swagger UI](https://swagger.io/) in your browser to explore and test the API.
