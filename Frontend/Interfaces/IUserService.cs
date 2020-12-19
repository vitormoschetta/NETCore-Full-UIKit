using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frontend.Models;

namespace Frontend.Interfaces
{
    public interface IUserService
    {
        Task<UserResult> Register(UserRegister user);
        Task<UserResult> RegisterAdmin(UserRegister user);
        Task<UserResult> Login(UserLogin user);
        Task<List<User>> GetInactivesFirstAccess();
        Task<UserResult> ActivateFirstAccess(Guid id, string role = "User");
        Task<UserResult> Delete(Guid id);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetByName(string name);
        Task<UserResult> UpdateRoleActive(User user);
        Task<UserResult> UpdatePassword(UserUpdatePassword userUpdatePassword);
        Task<List<User>> Search(string filter);
        Task<List<User>> SearchRequestAccess(string filter);
    }
}