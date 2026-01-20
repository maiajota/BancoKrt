using BancoKrt.Application.Services;
using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BancoKrt.Tests.Application;

public class ContaAppServiceTests
{
    private readonly IContaRepository _repository;
    private readonly ILogger<ContaAppService> _logger;
    private readonly ContaAppService _service;

    public ContaAppServiceTests()
    {
        _repository = Substitute.For<IContaRepository>();
        _logger = Substitute.For<ILogger<ContaAppService>>();

        _service = new ContaAppService(_repository, _logger);
    }

    [Fact]
    public async Task CriarContaAsync_DadosValidos_DeveChamarRepositorio()
    {
        // Arrange
        var nome = "Cliente Teste";
        var cpf = "12345678901";

        // Act
        await _service.CriarContaAsync(nome, cpf);

        // Assert
        await _repository.Received(1).AdicionarAsync(Arg.Any<Conta>());
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoContaNaoExiste_DeveRetornarNull()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();
        _repository.ObterPorIdAsync(idInexistente).Returns(Task.FromResult<Conta?>(null));

        // Act
        var resultado = await _service.ObterPorIdAsync(idInexistente);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AtualizarNomeAsync_ContaExistente_DeveChamarAtualizarNoRepositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contaExistente = new Conta("Nome Antigo", "12345678901");
        _repository.ObterPorIdAsync(id).Returns(contaExistente);

        // Act
        await _service.AtualizarAsync(id, "Nome Novo");

        // Assert
        Assert.Equal("Nome Novo", contaExistente.Nome);
        await _repository.Received(1).AtualizarAsync(contaExistente);
    }

    [Fact]
    public async Task DeletarAsync_ContaExistente_DeveMudarStatusParaInativo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var conta = new Conta("Cliente", "12345678901");
        _repository.ObterPorIdAsync(id).Returns(conta);

        // Act
        await _service.DeletarAsync(id);

        // Assert
        Assert.False(conta.IsAtivo);
        await _repository.Received(1).AtualizarAsync(conta);
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaDeDtos()
    {
        // Arrange
        var listaContas = new List<Conta>
    {
        new Conta("User 1", "11122233344"),
        new Conta("User 2", "55566677788")
    };
        _repository.ObterTodosAsync().Returns(listaContas);

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
        Assert.Contains(resultado, x => x.Nome == "User 1");
        Assert.Contains(resultado, x => x.Nome == "User 2");
    }

    [Fact]
    public async Task ReativarAsync_ContaInativa_DeveTornarAtivaNovamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var conta = new Conta("Cliente", "12345678901");
        conta.Desativar();
        _repository.ObterPorIdAsync(id).Returns(conta);

        // Act
        await _service.ReativarAsync(id);

        // Assert
        Assert.True(conta.IsAtivo);
        await _repository.Received(1).AtualizarAsync(conta);
    }

    [Fact]
    public async Task AtualizarAsync_ContaInexistente_NaoDeveChamarAtualizarNoRepositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.ObterPorIdAsync(id).Returns(Task.FromResult<Conta?>(null));

        // Act
        await _service.AtualizarAsync(id, "Qualquer Nome");

        // Assert
        await _repository.DidNotReceive().AtualizarAsync(Arg.Any<Conta>());
    }
}
