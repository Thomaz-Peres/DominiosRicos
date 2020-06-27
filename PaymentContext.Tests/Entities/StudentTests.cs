using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Address _address;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentTests()
        {
            _name = new Name("Buce", "Wayne");
            _document = new Document("45824232806", EDocumentType.cpf);
            _email = new Email("thomaz@dc.com");
            _address = new Address("Eminem", "49", "Slim Shady", "Marshall City", "SS", "MM", "08740580");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            var payment = new PayPalPayment("50504860", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "TOMENOS CORP", _document, _address, _email);

            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenActiveSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Valid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenAddSubscription()
        {
            var payment = new PayPalPayment("50504860", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "TOMENOS CORP", _document, _address, _email);

            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }
    }
}
