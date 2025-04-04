namespace Valida.Br.Tests;

public class CPFTests
{
    [Fact]
    public void IsValid_ShouldReturn_True()
    {
        // Arrange
        var cfp = new CPF(01337538167);

        // Act
        var result = cfp.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(555_555_555_55)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(123)]
    public void IsValid_ShouldReturn_False(long cpf)
    {
        // Arrange
        var cfp = new CPF(cpf);

        // Act
        var result = cfp.IsValid();

        // Assert
        Assert.False(result);
    }
}