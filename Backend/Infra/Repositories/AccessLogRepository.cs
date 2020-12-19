using Domain;
using Domains.Log.Entities;
using Domains.Subdomains.Log.Contracts.Repositories;
using Infra.Context;

namespace Infra.Repositories
{
    public class AccessLogRepository: IAccessLogRepository
    {
        private readonly AppDbContext _context;
        public AccessLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public string GetConnection() => Settings.ConnectionString();


        public void Register(AccessLog model)
        {
            _context.AccessLog.Add(model);
            _context.SaveChanges();
        }
    }
}