using ionpolis.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ionpolis.Controller
{
    public class Main_Controller
    {

        /// <summary>
        /// 메시지 box
        /// </summary>
        /// <param name="page">현재 페이지</param>
        /// <param name="message">띄울 메시지</param>
        public static void MessageBox(Page page, string message)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "Message Box", "<script language = 'javascript'>alert('" + message + "')</script>");
        }

        public static string Status()
        {
            string status = "";
            dashboardDb db = new dashboardDb();
            DataSet ds = new DataSet();
            ds = db.SelectDataList("select * from batch_status where left(create_date,10) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by create_date desc limit 1;");

            if (ds.Tables[0].Rows.Count != 0)
            {
                status = ds.Tables[0].Rows[0]["status"].ToString();
            }
            
            return status;
        }
    }
}