using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public Address(string street, string number, string neighborhood, string city, string state, string country, string zipCode)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Street, 5, "Address.Street", "A rua deve conter pelo menos 5 caracteres")
                .HasMinLen(Number, 1, "Address.Number", "O numero deve conter pelomenos 1 numero")
                .HasMinLen(City, 3, "Address.City", "A cidade deve conter pelomenos 3 numeros")
                .HasMinLen(State, 3, "Address.State", "Estado deve conter pelomenos 3 caracteres")
            );
        }

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
    }
}