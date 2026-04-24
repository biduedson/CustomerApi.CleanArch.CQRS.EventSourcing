

using Ardalis.Result;
using CustomerApi.Core;

namespace CustomerApi.Domain.ValueObjects;

public sealed record  Email
{
    private Email(string address) =>
        Address = address.ToLowerInvariant().Trim();

    public Email() { }
    public string Address { get; }

    public static Result<Email> Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            return Result<Email>.Error("O endereço de e-mail deve ser fornecido.");

        return !RegexPatterns.EmailIsValid.IsMatch(emailAddress)
            ? Result<Email>.Error("O endereço de e-mail é inválido.")
            : Result<Email>.Success(new Email(emailAddress));
    }
}
