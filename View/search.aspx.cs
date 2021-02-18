using ionpolis.Controller;
using ionpolis.Db;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace ionpolis.View
{
    public partial class search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Sdate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM");
            string Edate = DateTime.Now.ToString("yyyy-MM");

            if (!IsPostBack)
            {
                //사용자 메뉴 체크
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("login.aspx", false);
                }

                HdnDateS.Value = Sdate;
                HdnDateE.Value = Edate;

                setDropDown();
                Search("base_date", "DESC");
            }

            monthpickerS.Value = HdnDateS.Value;
            monthpickerE.Value = HdnDateE.Value;

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

        private void setDropDown()
        {
            dashboardDb db = new dashboardDb();

            DataSet dsSido = new DataSet();
            string query = "select '전체' as sigungu_name,'' as sigungu_full_name" +
                " union all" +
                " select sigungu_name, sigungu_full_name from sigungu where sigungu_sub_code = 'M';";
            dsSido = db.SelectDataList(query);

            drSido.DataSource = dsSido.Tables[0];
            drSido.DataTextField = dsSido.Tables[0].Columns["sigungu_name"].ToString();
            drSido.DataValueField = dsSido.Tables[0].Columns["sigungu_full_name"].ToString();

            drSido.DataBind();

            DataSet dsBrend = new DataSet();
            query = "select '' as comm_code,'전체' as comm_name" +
                " union all" +
                " select comm_code, comm_name from comm_code where comm_gubun = '1000';";
            dsBrend = db.SelectDataList(query);

            drBrend.DataSource = dsBrend.Tables[0];
            drBrend.DataTextField = dsBrend.Tables[0].Columns["comm_name"].ToString();
            drBrend.DataValueField = dsBrend.Tables[0].Columns["comm_code"].ToString();

            drBrend.DataBind();

            DataSet dsProduct = new DataSet();
            query = "select '' as comm_code,'전체' as comm_name" +
                " union all" +
                " select comm_code, comm_name from comm_code where comm_gubun = '2000';";
            dsProduct = db.SelectDataList(query);

            drProduct.DataSource = dsProduct.Tables[0];
            drProduct.DataTextField = dsProduct.Tables[0].Columns["comm_name"].ToString();
            drProduct.DataValueField = dsProduct.Tables[0].Columns["comm_code"].ToString();

            drProduct.DataBind();

            DataSet dsChannel = new DataSet();
            query = "select '' as comm_code,'전체' as comm_name" +
                " union all" +
                " select comm_name as comm_code, comm_name from comm_code where comm_gubun = '3000';";
            dsChannel = db.SelectDataList(query);

            drChannel.DataSource = dsChannel.Tables[0];
            drChannel.DataTextField = dsChannel.Tables[0].Columns["comm_name"].ToString();
            drChannel.DataValueField = dsChannel.Tables[0].Columns["comm_code"].ToString();

            drChannel.DataBind();
        }

        protected void drSido_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (drSido.SelectedValue == "")
            {
                drSigungu.Items.Clear();
                drSigungu.Items.Add(new ListItem() { Text = "전체", Value = "" });
            }
            else
            {
                dashboardDb db = new dashboardDb();
                DataSet dsSigungu = new DataSet();
                string query = "select '전체' as sigungu_name,'' as sigungu_code" +
                    " union all" +
                    " select sigungu_name, sigungu_name as sigungu_code from dong_code where sido_name = '" + drSido.SelectedValue + "' group by sigungu_name;";
                dsSigungu = db.SelectDataList(query);

                drSigungu.DataSource = dsSigungu.Tables[0];
                drSigungu.DataTextField = dsSigungu.Tables[0].Columns["sigungu_name"].ToString();
                drSigungu.DataValueField = dsSigungu.Tables[0].Columns["sigungu_code"].ToString();

                drSigungu.DataBind();
            }

            drDong.Items.Clear();
            drDong.Items.Add(new ListItem() { Text = "전체", Value = "" });
        }

        protected void drSigungu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drSigungu.SelectedValue == "")
            {
                drDong.Items.Clear();
                drDong.Items.Add(new ListItem() { Text = "전체", Value = "" });
            }
            else
            {
                dashboardDb db = new dashboardDb();
                DataSet dsDong = new DataSet();
                string query = "select '전체' as dong_name,'' as dong_code" +
                    " union all" +
                    " select dong_name, dong_name as dong_code from dong_code where sido_name = '" + drSido.SelectedValue + "' and sigungu_name = '" + drSigungu.SelectedValue + "';";
                dsDong = db.SelectDataList(query);

                drDong.DataSource = dsDong.Tables[0];
                drDong.DataTextField = dsDong.Tables[0].Columns["dong_name"].ToString();
                drDong.DataValueField = dsDong.Tables[0].Columns["dong_code"].ToString();

                drDong.DataBind();
            }
        }

        private void Search(string field, string sort)
        {
            dashboardDb db = new dashboardDb();

            string[] sigungu = drSigungu.SelectedValue.Split(' ');
            DataSet ds = new DataSet();

            StringBuilder sb = new StringBuilder();
            sb.Append(" select ");
            sb.Append("     a.base_date");
            sb.Append("     ,a.company");
            sb.Append("     ,b.comm_name as brand");
            sb.Append("     ,c.comm_name as product");
            sb.Append("     ,e.cust_name");
            sb.Append("     ,e.tel_no");
            sb.Append("     ,e.address");
            sb.Append(" from order_list_new a");
            sb.Append(" inner join comm_code b");
            sb.Append(" on a.brand_code = b.comm_code");
            sb.Append(" inner join comm_code c");
            sb.Append(" on a.product_code = c.comm_code");
            sb.Append(" inner join order_cust_addr d");
            sb.Append(" on a.cust_no = d.cust_no");
            sb.Append(" inner join order_cust e");
            sb.Append(" on a.cust_no = e.cust_no");
            sb.Append(" where left(a.base_date,7) between '"+ HdnDateS.Value + "' and '" + HdnDateE.Value + "'");
            sb.Append(" and a.company like CONCAT('%', '" + drChannel.SelectedValue + "' , '%')");
            sb.Append(" and a.brand_code like CONCAT('%', '" + drBrend.SelectedValue + "' , '%')");
            sb.Append(" and a.product_code like CONCAT('%', '" + drProduct.SelectedValue + "' , '%')");
            sb.Append(" and d.address_1 like CONCAT('%', '" + drSido.SelectedValue + "' , '%')");
            sb.Append(" and d.address_2 like CONCAT('%', '" + sigungu[0] + "' , '%')");
            sb.Append(" and d.address_3 like CONCAT('%', '" + (sigungu.Length == 1 ? "" : sigungu[1]) + "' , '%')");
            sb.Append(" and d.address_4 like CONCAT('%', '" + drDong.SelectedValue + "' , '%')");
            sb.Append(" order by " + field + " " + sort);

            ds = db.SelectDataList(sb.ToString());

            grdDetail.DataSource = ds;
            grdDetail.DataBind();
        }
        protected void btnSerch_Click(object sender, EventArgs e)
        {
            this.grdDetail.PageIndex = 0;
            Search("base_date", "DESC");
        }

        protected void grdDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.grdDetail.PageIndex = e.NewPageIndex;

            Search(SortColumn, SortDirection);
        }

        protected void grdDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
            SortColumn = e.SortExpression;

            Search(SortColumn, SortDirection);
        }

        public string SortColumn
        {
            get { return Convert.ToString(ViewState["SortColumn"]); }
            set { ViewState["SortColumn"] = value; }
        }

        public string SortDirection
        {
            get { return Convert.ToString(ViewState["SortDirection"]); }
            set { ViewState["SortDirection"] = value; }
        }
    }
}