using ionpolis.Controller;
using ionpolis.Db;
using System;

namespace ionpolis.View
{
    public partial class userAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //사용자 메뉴 체크
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("login.aspx", false);
                }
            }

            string status = Main_Controller.Status();
            if (status != "")
            {
                spStatus.Style.Add("display", "block");
                if (status == "P")
                {
                    dvStatusO.Style.Add("display", "block");
                }
                else if (status == "E")
                {
                    dvStatusG.Style.Add("display", "block");
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (USER_ID.Text == "")
            {
                Main_Controller.MessageBox(this, "ID를 입력해주세요.");
            }
            else if (USER_NAME.Text == "")
            {
                Main_Controller.MessageBox(this, "이름을 입력해주세요.");
            }
            else
            {
                dashboardDb db = new dashboardDb();
                bool check = db.Insert("insert into member values('" + USER_ID.Text + "','" + USER_ID.Text + "','" + USER_NAME.Text + "','admin','" + DateTime.Now.ToString() + "')");
                if (check)
                {
                    Main_Controller.MessageBox(this, "등록 완료.");
                }
                else
                {
                    Main_Controller.MessageBox(this, "등록 실패.");
                }
            }
        }
    }
}