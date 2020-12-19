using System;
using Shared;

namespace Domain.SubDomains.Authentication.Entities
{
    public class UserAuth : Notifiable
    {
        public UserAuth(string username, string password, string role = null, bool active = false)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
            Role = role;
            Active = active;

            Validate();
        }

        public UserAuth() { }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public string Role { get; private set; }
        public bool Active { get; private set; }


        public void AddHash(string hash, string salt)
        {
            Password = hash;
            Salt = salt;
        }

        public void HidePassword()
        {
            Password = string.Empty;
            Salt = string.Empty;
        }

        public void UpdatePassword(string password)
        {
            Password = password;

            Validate();
        }

        public void UpdateRoleActive(string role, bool active)
        {
            Role = role;
            Active = active;
        }

        public void Activate()
        {
            Active = true;
        }

        public void ActivateFirstAccess(string role)
        {
            Active = true;
            Role = role;
        }

        public void Inactivate()
        {
            Active = false;
        }


        public void Validate()
        {
            if (string.IsNullOrEmpty(Username))
            {
                AddNotification("Nome do Usuário é obrigatório! ");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                AddNotification("Senha é obrigatória! ");
                return;
            }

            if (Username.Length < 4)
                AddNotification("Nome do Usuário deve conter no mínimo 4 caracteres. ");

            if (Password.Length < 4)
                AddNotification("Senha deve conter no mínimo 4 caracteres. ");
           
        }
    }
}