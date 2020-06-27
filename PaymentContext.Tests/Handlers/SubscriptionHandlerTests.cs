using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // Red, Green, Refactor.

        [TestMethod]
        public void ShouldRetornErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "9999999999";
            command.Email = "hello@batman.com";
            command.BarCode = "123456789"; //  codigo de barras
            command.BoletoNumber = "1234654987";
            command.PaymentNumber = "123121";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "TOMENOS CORP";
            command.PayerDocument = "12346578911";
            command.PayerDocumentType = EDocumentType.cpf;
            command.PayerEmail = "batman@dc.com";
            command.Street = "dasd";
            command.Number = "das";
            command.Neighborhood = "dasd";
            command.City = "dasdsa";
            command.State = "dasdas";
            command.Country = "dasdasd";
            command.ZipCode = "12346578";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}
