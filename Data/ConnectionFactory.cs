using Microsoft.Practices.EnterpriseLibrary.Data;
using MySql.Data.MySqlClient;

namespace Data
{
    public static class ConnectionFactory
    {
        public static Database CreateDatabase()
        {
            string connectionString =
                "Server=localhost;Database=leavemanagementsystem;Uid=root;Pwd=root@123;";

            return new GenericDatabase(
                connectionString,
                MySqlClientFactory.Instance
            );
        }
    }
}