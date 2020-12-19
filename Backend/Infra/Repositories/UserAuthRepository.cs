using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain;
using Domain.SubDomains.Authentication.Contracts.Repositories;
using Domain.SubDomains.Authentication.Entities;
using Infra.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly AppDbContext _context;
        public UserAuthRepository(AppDbContext context)
        {
            _context = context;
        }
        public string GetConnection() => Settings.ConnectionString();


        public void Register(UserAuth model)
        {
            _context.UserAuth.Add(model);
            _context.SaveChanges();
        }

        public List<UserAuth> GetInactivesFirstAccess()
        {
            string query = "";
            query += " select U.Id, U.Username, U.Role, U.Active";
            query += " from UserAuth as U ";
            query += " where U.Role is null and U.Active = 0";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                return _dapper.Query<UserAuth>(query).ToList();
            }
        }

        public UserAuth Login(string userName, string password)
        {
            return _context.UserAuth
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == userName && x.Password == password);
        }

        public UserAuth GetSalt(string userName)
        {
            return _context.UserAuth
                .AsNoTracking()
                .SingleOrDefault(x => x.Username == userName);
        }

        public void UpdateRoleActive(UserAuth model)
        {
            string query = "";
            query += $" update UserAuth set Active = '{model.Active}', ";
            query += $" role = '{model.Role}' ";
            query += $" where Id = '{model.Id}' ";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                _dapper.Execute(query);
            }
        }

        public void UpdatePassword(Guid id, string password)
        {
            string query = "";
            query += $" update UserAuth set Password = '{password}' ";
            query += $" where Id = '{id}' ";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                _dapper.Execute(query);
            }
        }

        public void Delete(Guid id)
        {
            var model = _context.UserAuth.FirstOrDefault(x => x.Id == id);
            _context.Remove(model);
            _context.SaveChanges();
        }

        public bool Exists(string name)
        {
            var model = _context.UserAuth.FirstOrDefault(x => x.Username == name);
            if (model != null) return true;
            return false;
        }

        public bool ExistsUpdate(string name, Guid id)
        {
            var model = _context.UserAuth.FirstOrDefault(x => x.Username == name && x.Id != id);
            if (model != null) return true;
            return false;
        }

        public List<UserAuth> GetAll()
        {
            string query = "";
            query = " select U.Id, U.Username, U.Role, U.Active ";
            query += " from UserAuth as U ";
            query += " where U.Role is not null ";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                return _dapper.Query<UserAuth>(query).ToList();
            }

        }

        public UserAuth GetById(Guid id)
        {
            string query = "";
            query = " select U.Id, U.Username, U.Role, U.Active ";
            query += " from UserAuth as U ";
            query += $" where U.Id = '{id}' ";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                return _dapper.QuerySingleOrDefault<UserAuth>(query);
            }

        }

        public UserAuth GetByName(string name)
        {
            string query = "";
            query = " select U.Id, U.Username, U.Role, U.Active ";
            query += " from UserAuth as U ";
            query += $" where U.Username = '{name}' ";

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                return _dapper.QuerySingleOrDefault<UserAuth>(query);
            }

        }

        public List<UserAuth> Search(string filter)
        {
            string query = "";
            query = " select U.Id, U.Username, U.Role, U.Active ";
            query += " from UserAuth as U ";

            filter = filter.ToLower();

            if (filter == "ativo" || filter == "inativo")
            {
                if (filter == "ativo")
                    filter = "1";
                else
                    filter = "0";
                query += $" where U.Active = '{filter}' ";
            }
            else
            {
                query += $" where U.Username like '%{filter}%' ";
                query += $" or U.Role like '%{filter}%' ";
            }

            using (var _dapper = new SqlConnection(GetConnection()))
            {
                return _dapper.Query<UserAuth>(query).ToList();
            }

        }

    }
}