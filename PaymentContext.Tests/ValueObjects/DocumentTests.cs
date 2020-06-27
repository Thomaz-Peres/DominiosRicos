using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class DocumentTests
    {
        // Red, Green, Refactor.
        [TestMethod]
        public void ShouldRetornErrorWhenCNPJIsInvalid()     //  CNPJ
        {
            var doc = new Document("123", EDocumentType.cnpj);
            Assert.IsTrue(doc.Invalid);
        }

        [TestMethod]
        public void ShouldRetornSuccessWhenCNPJIsValid()     //  CNPJ
        {
            var doc = new Document("03860607000130", EDocumentType.cnpj);
            Assert.IsTrue(doc.Valid);
        }

        [TestMethod]
        public void ShouldRetornErrorWhenCPFIsInvalid()     //  CPF
        {
            var doc = new Document("123", EDocumentType.cpf);
            Assert.IsTrue(doc.Invalid);
        }

        [TestMethod]
        public void ShouldRetornSuccessWhenCPFIsValid()     //  CPF
        {
            var doc = new Document("45824232806", EDocumentType.cpf);
            Assert.IsTrue(doc.Valid);
        }
    }
}
