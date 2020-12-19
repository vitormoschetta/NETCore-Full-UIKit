using System;
using System.Collections.Generic;
using System.Linq;
using Domain.SubDomains.Authentication.Contracts.Repositories;
using Domain.SubDomains.Authentication.Entities;
using Shared;

namespace Tests.Mock
{
    public class FakeUserAuthRepository : IUserAuthRepository
    {
        private readonly List<UserAuth> List;
        public FakeUserAuthRepository()
        {
            List = new List<UserAuth>();

            // Usuario Admin: 
            var user = new UserAuth("admin", "123456", "Admin", true);
            var salt = Salt.Create();
            var hash = Hash.Create(user.Password, salt);
            user.AddHash(hash, Convert.ToBase64String(salt));
            List.Add(user);

            // Usuario comum ativo:
            user = new UserAuth("userActive", "123456", "User", true);
            salt = Salt.Create();
            hash = Hash.Create(user.Password, salt);
            user.AddHash(hash, Convert.ToBase64String(salt));
            List.Add(user);

            // Usuario comum inativo:
            user = new UserAuth("userInactive", "123456", "User", false);
            salt = Salt.Create();
            hash = Hash.Create(user.Password, salt);
            user.AddHash(hash, Convert.ToBase64String(salt));
            List.Add(user);

            // Usuario cadastrado, aguardando liberação de acesso (feito pelo Admin):
            user = new UserAuth("userNew", "123456");
            salt = Salt.Create();
            hash = Hash.Create(user.Password, salt);
            user.AddHash(hash, Convert.ToBase64String(salt));
            List.Add(user);
        }
        public void Register(UserAuth model)
        {
            List.Add(model);
        }

        public List<UserAuth> GetInactivesFirstAccess()
        {
            return List.Where(x => x.Active == false && x.Role == null).ToList();
        }

        public void UpdateRoleActive(UserAuth model)
        {
            var user = List.SingleOrDefault(x => x.Id == model.Id);
            if (user == null)
                return;

            List.Remove(user);
            List.Add(model);
        }

        public UserAuth Login(string userName, string password)
        {
            return List.SingleOrDefault(x => x.Username == userName && x.Password == password);
        }

        public UserAuth GetById(Guid id)
        {
            return List.SingleOrDefault(x => x.Id == id);
        }

        public UserAuth GetByName(string name)
        {
            return List.SingleOrDefault(x => x.Username == name);
        }

        public void Activate(Guid id, string role)
        {
            var user = List.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return;

            var newUser = new UserAuth(user.Username, user.Password, role, true);

            List.Remove(user);
            List.Add(newUser);
        }

        public void Inactivate(Guid id, string role)
        {
            var user = List.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return;

            var newUser = new UserAuth(user.Username, user.Password, role, false);

            List.Remove(user);
            List.Add(newUser);
        }

        public void UpdatePassword(Guid id, string password)
        {
            var user = List.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return;

            var newUser = new UserAuth(user.Username, password, user.Role, true);

            List.Remove(user);
            List.Add(newUser);
        }

        public void Delete(Guid id)
        {
            var user = List.SingleOrDefault(x => x.Id == id);
            if (user != null)
                List.Remove(user);
        }

        public bool Exists(string userName)
        {
            var user = List.SingleOrDefault(x => x.Username == userName);
            if (user == null)
                return false;

            return true;
        }

        public bool ExistsUpdate(string userName, Guid id)
        {
            var item = List.FirstOrDefault(x => x.Username == userName && x.Id != id);
            if (item != null)
                return true;

            return false;
        }

        public List<UserAuth> GetAll()
        {
            return List;
        }

        public UserAuth GetSalt(string userName)
        {
            return List.SingleOrDefault(x => x.Username == userName);
        }

        public List<UserAuth> Search(string filter)
        {
            return List.Where(x => x.Username.Contains(filter)).ToList();
        }
    }
}