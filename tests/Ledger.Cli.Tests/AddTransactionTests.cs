using Ledger.Cli.UseCases.Commands;
using Ledger.Domain.Entities;
using Ledger.Domain.Enums;
using Ledger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace Ledger.Cli.Tests;

public class AddTransactionTests
{
    private readonly ITransactionRepository _repository;
    private readonly AddTransaction _useCase;

    public AddTransactionTests()
    {
        _repository = Substitute.For<ITransactionRepository>();
        _useCase = new AddTransaction(_repository);
    }

    [Fact]
    public async Task Execute_ValidTransaction_ShouldReturnId()
    {
        // Arrange
        var expectedId = 42L;
        _repository.AddAsync(Arg.Any<Transaction>()).Returns(expectedId);

        var input = new AddTransactionInput(10000, Category.OtherIncome, "test note");

        // Act
        var result = await _useCase.ExecuteAsync(input);

        // Assert
        Assert.Equal(expectedId, result);
    }
}
