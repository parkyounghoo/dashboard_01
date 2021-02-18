using ionpolis.Controller;
using ionpolis.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ionpolis.View
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                dashboardDb db = new dashboardDb();
                ds = db.SelectDataList("select * from member where id = '" + txtid.Text + "' and password = '" + txtpw.Text + "'");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    Main_Controller.MessageBox(this, "로그인에 실패 했습니다.");
                }
                else
                {
                    Session["USER_ID"] = txtid.Text;

                    if (Request.FilePath == "/View/login")
                    {
                        Response.Redirect("dashboard.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("View/dashboard.aspx", false);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}