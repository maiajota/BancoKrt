namespace BancoKrt.Domain.Entities;

public class Conta(string nome, string cpf)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = nome;
    public string Cpf { get; private set; } = cpf;
    public bool IsAtivo { get; private set; } = true;

    public void Update(string nome, bool status)
    {
        Nome = nome;
        IsAtivo = status;
    }
}
