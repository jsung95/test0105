using System;
using Oracle.ManagedDataAccess.Client;

namespace EasyProject.Dao
{
    public class CommonDBConn
    {
        /// /////////////////////////////////////////
        // User Connect Information//////////////////
        protected static readonly string user = "ADMIN";
        protected static readonly string password = "Oracle12345!";
        protected static readonly string ds = "db202112031025_high";

        protected static readonly string connectionString = $"User Id={user};Password={password};Data Source={ds};";
        /// /////////////////////////////////////////
        /// /////////////////////////////////////////


        //protected OracleConnection conn = new OracleConnection(connectionString);
        //protected OracleCommand cmd = new OracleCommand();


    }//class

}
