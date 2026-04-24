

using CustomerApi.Core.SharedKernel;
using CustomerApi.Domain.Entities.CustomerAggregate.Events;
using CustomerApi.Domain.ValueObjects;

namespace CustomerApi.Domain.Entities.CustomerAggregate;

public class Customer : BaseEntity, IAggregateRoot
{
    private bool _isDeleted;


    public Customer(string firstName, string lastName, EGender gender, Email email, DateTime dateOfBirth) 
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
        DateOfBirth = dateOfBirth;

        AddDomainEvents(new CustomerCreatedEvent(Id, firstName, lastName, gender, email.Address, dateOfBirth));
    }

    public Customer() { }


    public string FirstName { get;}
    public string LastName { get;}
    public EGender Gender { get; }
    public Email Email { get; private set; }
    public DateTime DateOfBirth { get; }

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
