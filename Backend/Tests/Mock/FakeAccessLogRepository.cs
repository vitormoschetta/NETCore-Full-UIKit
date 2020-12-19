using System.Collections.Generic;
using Domains.Log.Entities;
using Domains.Subdomains.Log.Contracts.Repositories;

namespace Tests.Mock
{
    public class FakeAccessLogRepository : IAccessLogRepository
    {
        private readonly List<AccessLog> List;
        public FakeAccessLogRepository()
        {
            List = new List<AccessLog>();           
        }
        

        public void Register(AccessLog model)
        {
            List.Add(model);
        }
    }
}