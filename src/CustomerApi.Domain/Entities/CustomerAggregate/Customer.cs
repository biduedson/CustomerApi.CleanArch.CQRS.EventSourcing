

using CustomerApi.Core.SharedKernel;
using CustomerApi.Domain.Entities.CustomerAggregate.Events;
using CustomerApi.Domain.Exceptions;
using CustomerApi.Domain.ValueObjects;

namespace CustomerApi.Domain.Entities.CustomerAggregate;

public class Customer : BaseEntity, IAggregateRoot
{
    private bool _isDeleted;


    private Customer(string firstName, string lastName, EGender gender, Email email, DateTime dateOfBirth) 
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
        DateOfBirth = dateOfBirth;

        AddDomainEvents(new CustomerCreatedEvent(Id, firstName, lastName, gender, email.Address, dateOfBirth));
    }

    private Customer() { }


    public string FirstName { get;}
    public string LastName { get;}
    public EGender Gender { get; }
    public Email Email { get; private set; }
    public DateTime DateOfBirth { get; }


    public static Customer Create(string firstName, string lastName, EGender gender,string email, DateTime dateOfBirth) 
    {
       if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty.");

       if(string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty.");

       if(string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

       var emailCreated = Email.Create(email);


        return new Customer(firstName, lastName, gender, emailCreated, dateOfBirth);
    }
    public void ChangeEmail(Email newEmail)
    {
        if (Email.Equals(newEmail))
            return;
       
       Email = newEmail;
       
        AddDomainEvents(new CustomerUpdatedEvent(Id, FirstName, LastName, Gender, Email.Address, DateOfBirth));
    }

    public  void Delete()
    {
        if (_isDeleted)
            return;

        _isDeleted = true;

        AddDomainEvents(new CustomerUpdatedEvent(Id, FirstName, LastName, Gender, Email.Address, DateOfBirth));
    }
}
