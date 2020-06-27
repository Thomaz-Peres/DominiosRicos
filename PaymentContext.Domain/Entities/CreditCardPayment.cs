using System;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class CreditCardPayment : Payment
    {
        public CreditCardPayment(
            string cardHolderName, 
            string cardNumber, 
            string lastTransactionNumber, 
            DateTime paidDate, 
            DateTime expireDate, 
            decimal total, 
            decimal totalPaid, 
            string payer,
            Document document,
            Address address,
            Email email) : base(
                paidDate, 
                expireDate, 
                total, 
                totalPaid, 
                payer, 
                document, 
                address, 
                email)
        {
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            LastTransactionNumber = lastTransactionNumber;

            AddNotifications(new Contract()
                .Requires()
                .AreNotEquals(document, cardHolderName, "CreditCardPayment.cadHolderName", "O nome do cartao e do documento, sao diferentes")
            );
        }

        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }  //  guardar os ultimos 4 digitos do cartao
        public string LastTransactionNumber { get; private set; }   //  numero da ultima trasaçao feita
    }
}