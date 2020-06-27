using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Student : Entity
    {
        private IList<Subscription> _subscriptions;
        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscriptions = new List<Subscription>();


            //  concatenando as notificaçoes.
            AddNotifications(name, document, email);
        }

        public Name Name { get; private set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions { get { return _subscriptions.ToArray(); } }

        public void AddSubscription(Subscription subscription)      //  adicionar uma Assinatura.
        {
            //  Vendo se o aluno tem alguma subscriçao feita
            var hasSubscriptionActive = false;
            foreach(var sub in _subscriptions)
            {
                if(sub.Active){
                    hasSubscriptionActive = true;
                }

                 //dois modos de ver se existe uma subscription ativa

                AddNotifications(new Contract()
                    .Requires()
                    .IsFalse(hasSubscriptionActive, "Student.Subscription", "O usuario ja tem uma assinatura ativa")
                    .AreEquals(0, subscription.Payments.Count, "Student.Subscription.Payments", "esta assinatura nao possui pagamentos.")
                );

                // if(hasSubscriptionActive){
                //     AddNotification("Student.Subscription", "O usuario ja tem uma assinatura ativa");
                // }
            }
        }
    }
}