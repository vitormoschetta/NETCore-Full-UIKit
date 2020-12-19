using System;
using System.Collections.Generic;
using Domain.SubDomains.Authentication.Entities;

namespace Domain.SubDomains.Authentication.Contracts.Repositories
{
    public interface IUserAuthRepository
    {
        void Register(UserAuth model);
        UserAuth GetSalt(string userName);
        UserAuth Login(string userName, string password);
        void UpdateRoleActive(UserAuth model);
        void UpdatePassword(Guid id, string password);
        void Delete(Guid id);
        UserAuth GetById(Guid id);
        UserAuth GetByName(string name);
        List<UserAuth> GetAll();
        List<UserAuth> GetInactivesFirstAccess();
        bool Exists(string userName);
        bool ExistsUpdate(string userName, Guid id);
        List<UserAuth> Search(string param);

    }
}