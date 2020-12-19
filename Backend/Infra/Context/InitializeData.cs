using System;
using System.Linq;
using Domain.SubDomains.Authentication.Entities;
using Domains.Log.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Infra.Context
{
    public static class InitializeData
    {
        public static void InitializeUsers(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
               serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.UserAuth.Any())  // <-- Se já possui dados, não prossegue
                    return;

                // Usuario Admin: 
                var user = new UserAuth("admin", "1234", "Admin", true);
                var salt = Salt.Create();
                var hash = Hash.Create(user.Password, salt);
                user.AddHash(hash, Convert.ToBase64String(salt));
                context.Add(user);
                context.SaveChanges();

                // Usuario comum ativo:
                user = new UserAuth("usuario", "1234", "User", true);
                salt = Salt.Create();
                hash = Hash.Create(user.Password, salt);
                user.AddHash(hash, Convert.ToBase64String(salt));
                context.Add(user);
                context.SaveChanges();

                // Usuario comum inativo:
                user = new UserAuth("inativo", "1234", "User", false);
                salt = Salt.Create();
                hash = Hash.Create(user.Password, salt);
                user.AddHash(hash, Convert.ToBase64String(salt));
                context.Add(user);
                context.SaveChanges();

                // Usuario cadastrado, aguardando liberação de acesso (feito pelo Admin):
                user = new UserAuth("novo", "1234");
                salt = Salt.Create();
                hash = Hash.Create(user.Password, salt);
                user.AddHash(hash, Convert.ToBase64String(salt));
                context.Add(user);
                context.SaveChanges();
            }
        }
      

        public static void InitializeAccesLog(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
               serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.AccessLog.Any())  // <-- Se já possui dados, não prossegue
                    return;

                var log = new AccessLog("Login", DateTime.Now, "usuario teste", null, null);

                context.Add(log);
                context.SaveChanges();

            }
        }
    }
}