namespace BancoKrt.Application.DTOs;

public record ContaDto(
    Guid Id, 
    string Nome, 
    string Cpf, 
    bool IsAtivo
);
