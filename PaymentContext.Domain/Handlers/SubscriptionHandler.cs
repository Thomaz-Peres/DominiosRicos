using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //  Fail Fast Validation
            command.Validate();
            if(command.Invalid){
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar sua assinatura");
            }

            //  Verificar se Documento ja esta cadastrado
            if(_repository.DocumentsExists(command.Document)){
                AddNotification("Document", "Este CPF ja esta em uso");
            }

            //  Verificar se E=mail ja esta cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Email", "Este E-mail ja esta em uso");
            }

            //  Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.cpf);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);


            //  Gerar as entidades
            var student = new Student(name, document,   email);
            var subscription = new Subscription(DateTime.Now.AddDays(30));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid,
                command.Payer, new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email
            );

            //  Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //  Agrupar as validaçoes
            AddNotifications(name, document, email, address, student, subscription, payment);

            //  Salvar as Informaçoes
            _repository.CreateSubscription(student);

            //  Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao TOMENOS ENTERPRISE", "Sua assinatura foi criada");

            //  Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            //  Verificar se Documento ja esta cadastrado
            if (_repository.DocumentsExists(command.Document))
            {
                AddNotification("Document", "Este CPF ja esta em uso");
            }

            //  Verificar se E=mail ja esta cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Email", "Este E-mail ja esta em uso");
            }               

            //gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.cpf);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades 
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            //So muda a implemetacao
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate, 
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType),
                    address,
                    email
            );
            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as Validacoes
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificaçoes
            if(Invalid){
                return new CommandResult(false, "nao foi possivel realizar sua assinatura");
            }
            //Salvar as informações
            _repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao TOMENOS ENTERPRISE", "Sua assinatura foi criada");

            //Retornar informaçoes
            return new CommandResult(true, "Assinatura realizada com Sucesso");
        }
    }
}