using System;
using Domain.SubDomains.Authentication.Commands;
using Domain.SubDomains.Authentication.Contracts.Handlers;
using Domain.SubDomains.Authentication.Contracts.Repositories;
using Domain.SubDomains.Authentication.Entities;
using Domains.Authentication.Commands.UserAuthCommands;
using Domains.Log.Entities;
using Domains.Subdomains.Log.Contracts.Repositories;
using Shared;

namespace Domain.SubDomains.Authentication.Handlers
{
    public class UserAuthHandler : Notifiable, IUserAuthHandler
    {
        private readonly IUserAuthRepository _repository;
        private readonly IAccessLogRepository _log;
        public UserAuthHandler(IUserAuthRepository repository, IAccessLogRepository log)
        {
            _repository = repository;
            _log = log;
        }

        public CommandResult Register(RegisterUserAuthCommand command)
        {
            var exist = _repository.Exists(command.Username);
            if (exist)
                AddNotification("Já existe um Usuario cadastrado com esse Nome. ");

            var user = new UserAuth(command.Username, command.Password);

            AddNotifications(user);

            if (Invalid)
                return new CommandResult(false, GroupNotifications.Group(Notifications), command);

            // Add Hash e Salt
            var salt = Salt.Create();
            var hash = Hash.Create(user.Password, salt);
            if (!Hash.Validate(user.Password, salt, hash))
                AddNotification("Erro na geração do Hash. ");

            user.AddHash(hash, Convert.ToBase64String(salt));

            _repository.Register(user);

            var log = new AccessLog(
                "Register",
                DateTime.Now,
                "auto registro",
                "UserAuth",
                $"Nome usuario registrado: {command.Username}");

            _log.Register(log);

            user.HidePassword();

            return new CommandResult(true, "Cadastro realizado. ", user);
        }

        public CommandResult RegisterAdmin(RegisterAdminUserAuthCommand command, string userIdentity)
        {
            var exist = _repository.Exists(command.Username);
            if (exist)
                AddNotification("Já existe um usuario cadastrado com esse nome. ");

            var user = new UserAuth(command.Username, command.Password, command.Role, command.Active);

            AddNotifications(user);

            if (Invalid)
                return new CommandResult(false, GroupNotifications.Group(Notifications), command);

            // Add Hash e Salt
            var salt = Salt.Create();
            var hash = Hash.Create(user.Password, salt);
            if (!Hash.Validate(user.Password, salt, hash))
                AddNotification("Erro na geração do Hash. ");

            user.AddHash(hash, Convert.ToBase64String(salt));

            _repository.Register(user);

            var log = new AccessLog(
                "Register",
                DateTime.Now,
                userIdentity,
                "UserAuth",
                $"Nome usuario registrado: {command.Username}");

            _log.Register(log);

            user.HidePassword();

            return new CommandResult(true, "Cadastro realizado. ", user);
        }



        public CommandResult ActivateFirstAccess(ActivateUserCommand command, string userIdentity)
        {
            var user = _repository.GetById(command.Id);
            if (user == null)       
                return new CommandResult(false, "Usuário não encontrado. ", command);            

            user.ActivateFirstAccess(command.Role);

            _repository.UpdateRoleActive(user);

            var log = new AccessLog(
                "PrimeiroAcesso",
                DateTime.Now,
                userIdentity,
                "UserAuth",
                $"acesso liberado: {command.Id}");

            _log.Register(log);

            user.HidePassword();

            return new CommandResult(true, "Usuário Ativo. ", user);
        }


        public CommandResultToken Login(string userName, string password)
        {
            var user = _repository.GetSalt(userName);
            if (user == null)
                return new CommandResultToken(false, "Login inválido. ", null);

            var salt_tabela = user.Salt;
            byte[] salt = Convert.FromBase64String(salt_tabela);
            var hashPassword = Hash.Create(password, salt); // <-- monta hash para comparação / login

            user = _repository.Login(userName, hashPassword);
            if (user == null)
                return new CommandResultToken(false, "Login inválido. ", null);

            if (user.Active == false && user.Role == null)
                AddNotification("Aguardando liberação de acesso. ");
            else if (user.Active == false)
                AddNotification("Usuário inativo. Contacte o Administrado. ");

            if (Invalid)
                return new CommandResultToken(false, GroupNotifications.Group(Notifications), null);

            var log = new AccessLog(
                "Login",
                DateTime.Now,
                userName,
                null,
                null);

            _log.Register(log);

            user.HidePassword();

            return new CommandResultToken(true, "Login efetuado com sucesso! ", user);
        }

        public CommandResult UpdateRoleActive(UpdateRoleActiveCommand command, string userIdentity)
        {
            var user = _repository.GetById(command.Id);
            if (user == null) 
                return new CommandResultToken(false, "Usuário não encontrado. ", command);
            
            user.UpdateRoleActive(command.Role, command.Active);

            _repository.UpdateRoleActive(user);

            var log = new AccessLog(
                "UpdateRoleActive",
                DateTime.Now,
                userIdentity,
                "UserAuth",
                $"Id acesso atualizado: {command.Id}");

            _log.Register(log);

            user.HidePassword();

            return new CommandResult(true, "Atualizado. ", user);
        }

        public CommandResult Delete(Guid id, string userIdentity)
        {
            var user = _repository.GetById(id);
            if (user == null)
                return new CommandResult(false, "Usuário não encontrado. ", null);            

            _repository.Delete(user.Id);

            var log = new AccessLog(
                "Delete",
                DateTime.Now,
                userIdentity,
                "UserAuth",
                $"Usuário excluído: {user.Username}");

            _log.Register(log);

            return new CommandResult(true, "Usuário excluído. ", user);
        }

        public CommandResult UpdatePassword(UpdatePasswordUserCommand command, string userIdentity)
        {
            // verificação da validade do usuario e senha
            var user = _repository.GetSalt(command.Username);
            if (user == null)
                return new CommandResult(false, "Login inválido. ", command);            

            var salt_tabela = user.Salt;
            byte[] salt = Convert.FromBase64String(salt_tabela);
            var hashPassword = Hash.Create(command.Password, salt);

            user = _repository.Login(command.Username, hashPassword);
            if (user == null)
                return new CommandResult(false, "Senha antiga não confere. ", command);            

            // criacao de novo hash para a nova senha
            hashPassword = Hash.Create(command.NewPassword, salt);

            user.UpdatePassword(hashPassword);

            AddNotifications(user);
            if (Invalid)
                return new CommandResult(false, GroupNotifications.Group(Notifications), command);

            _repository.UpdatePassword(user.Id, hashPassword);

            var log = new AccessLog(
                "UpdatePassword",
                DateTime.Now,
                userIdentity,
                "UserAuth",
                $"Usuário alterado: {command.Username}");

            _log.Register(log);

            user.HidePassword();

            return new CommandResultToken(true, "Senha alterada com sucesso! ", user);
        }
    }
}