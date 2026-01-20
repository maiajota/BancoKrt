using BancoKrt.Domain.Entities;
using Xunit;

namespace BancoKrt.Tests.Domain;

public class ContaTests
{
    [Fact]
    public void CriarConta_DeveDefinirPropriedadesCorretamente()
    {
        // Arrange (Preparar)
        var nome = "Utilizador Teste";
        var cpf = "12345678901";

        // Act (Executar)
        var conta = new Conta(nome, cpf);

        // Assert (Verificar)
        Assert.Equal(nome, conta.Nome);
        Assert.Equal(cpf, conta.Cpf);
        Assert.True(conta.IsAtivo);
    }

    [Fact]
    public void Desativar_ContaAtiva_DeveFicarInativa()
    {
        // Arrange
        var conta = new Conta("Teste", "12345678901");

        // Act
        conta.Desativar();

        // Assert
        Assert.False(conta.IsAtivo);
    }

    [Fact]
    public void Reativar_ContaInativa_DeveFicarAtiva()
    {
        // Arrange
        var conta = new Conta("Teste", "12345678901");
        conta.Desativar();

        // Act
        conta.Reativar();

        // Assert
        Assert.True(conta.IsAtivo);
    }
}
