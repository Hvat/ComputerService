using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ComputerService
{
    internal class DataBase
    {
        readonly SqlConnection sqlConnection = new SqlConnection(@"Data Source = HVAT;
Initial Catalog = Computer_Service; Integrated Security = True");


        public void openConection()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Closed )
            {
                sqlConnection.Open();
            }
        }

        public void closeConection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConection()
        { 
            return sqlConnection; 
        }
    }
}
