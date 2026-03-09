# Ledger CLI

A personal finance CLI tool for tracking income and expenses. Built with .NET 10, SQLite, and Spectre.Console.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Getting Started

```bash
# Clone the repository
git clone <repo-url>
cd ledger-cli

# Restore dependencies
dotnet restore

# Apply database migrations (creates ledger.db at solution root)
dotnet ef database update --project src/Ledger.Infrastructure --startup-project src/Ledger.Cli

# Run the app
dotnet run --project src/Ledger.Cli
```

## Usage

The CLI supports both single commands and an interactive REPL mode.

### Single command

```bash
# Add a transaction
dotnet run --project src/Ledger.Cli -- add 50000 --note "lunch" --category FoodAndDrink

# List transactions by month
dotnet run --project src/Ledger.Cli -- list 2026 3
```

### Interactive REPL

```bash
dotnet run --project src/Ledger.Cli

# Inside the REPL:
ledger> add 50000 --note "lunch"
ledger> list 2026 3
ledger> exit
```

## Project Structure

```
src/
  Ledger.Domain/           # Entities, enums, repository interfaces
  Ledger.Infrastructure/   # EF Core DbContext, migrations, repositories
  Ledger.Cli/              # CLI commands, use cases (CQRS), DI setup
    Commands/              # Spectre.Console CLI command handlers
    UseCases/
      Commands/            # Write operations (with interface for mocking)
      Queries/             # Read operations (direct DbContext access)

tests/
  Ledger.Domain.Tests/           # Unit tests for domain logic
  Ledger.Cli.Tests/              # Unit tests for command use cases (mocked repos)
  Ledger.Infrastructure.Tests/   # Integration tests with in-memory SQLite
    Repositories/                # Repository tests
    Queries/                     # Query use case tests
```

## Development Setup

### Git hooks

The project uses a pre-commit hook that auto-formats staged C# files. Configure it after cloning:

```bash
git config core.hooksPath .githooks
```

### Code formatting

Formatting rules are defined in `.editorconfig`. To manually format the codebase:

```bash
dotnet format
```

### Database Migrations

The project uses EF Core with SQLite. The database file (`ledger.db`) is created at the solution root.

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project src/Ledger.Infrastructure --startup-project src/Ledger.Cli

# Apply migrations
dotnet ef database update --project src/Ledger.Infrastructure --startup-project src/Ledger.Cli
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run a specific test project
dotnet test tests/Ledger.Domain.Tests
dotnet test tests/Ledger.Cli.Tests
dotnet test tests/Ledger.Infrastructure.Tests
```
