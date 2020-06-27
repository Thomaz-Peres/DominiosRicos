using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Subscription : Entity
    {
        private IList<Payment> _payments;
        public Subscription(DateTime? expireDate)
        {
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            ExpireDate = expireDate;
            Active = true;  //  true pois vai vir sempre com um pagamento como PAGO.
            _payments = new List<Payment>();
        }

        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }    // data ultima atualizaçao
        public DateTime? ExpireDate { get; private set; }
        public bool Active { get ; private set; }   //  so pode ter uma assinatura ativa por mês
        public IReadOnlyCollection<Payment> Payments { get { return _payments.ToArray();} } //  lista de pagamentos

        public void AddPayment(Payment payment)     //  Adicionar um pagamento
        {
            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "A data do pagamento tem que ser futura")
            );

            // if (valid)   //  So vai adicionar o pagamento se for válido.
            _payments.Add(payment);
        }

        public void Activate()
        {
            Active = true;
            LastUpdateDate = DateTime.Now;
        }

        public void Inactivate()
        {
            Active = false;
            LastUpdateDate = DateTime.Now;
        }
    }
}