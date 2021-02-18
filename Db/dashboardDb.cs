using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ionpolis.Db
{
    public class dashboardDb
    {
        public DataSet SelectDataList(string query)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                conn.Open();
                MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
                adpt.Fill(ds);

                return ds;
            }
        }

        public bool Insert(string query)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}