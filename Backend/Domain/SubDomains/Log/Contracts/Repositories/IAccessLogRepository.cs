using Domains.Log.Entities;

namespace Domains.Subdomains.Log.Contracts.Repositories
{
    public interface IAccessLogRepository
    {
        void Register(AccessLog model);
    }
}