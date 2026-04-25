


using CustomerApi.Core;
using CustomerApi.Domain.Exceptions;

namespace CustomerApi.Domain.ValueObjects;

public sealed record  Email
{
    private Email(string address) =>
        Address = address.ToLowerInvariant().Trim();

    private Email() { }
    public string Address { get; }

    public static Email Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email cannot be empty.");

        if(!RegexPatterns.EmailIsValid.IsMatch(emailAddress))
            throw new DomainException("Email format is invalid.");
        
        return new Email(emailAddress);
    }

    public override string ToString() => Address;
}
