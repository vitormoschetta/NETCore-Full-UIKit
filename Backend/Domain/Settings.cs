using System;

namespace Domain
{
    public static class Settings
    {
        public static string ConnectionString()
        {                    
            return "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=NETCoreDB;Integrated Security=true;";        }

        public static string Secret = "fedaf7d8863b48e197b9287d492b708e";

    }
}
