using DatabaseProvider;
using DatabaseProvider.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopClient
{
    public static class DatabaseProvider
    {
        public static IDatabaseController Database;

        public static bool Init(string databaseType, string[] connectArgs)
        {
            if (databaseType == "MySQL")
            {
                Database = new MySQLController();
            } else if (databaseType == "LiteDB")
            {
                //Database = new LiteDBController();
            }
            Database.ConnectToDatabase(connectArgs);
            return true;
        }
    }
}
