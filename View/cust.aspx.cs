using ionpolis.Controller;
using ionpolis.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ionpolis.View
{
    public partial class cust : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Sdate = DateTime.Now.ToString("yyyy-MM");
            string sido = "";
            string sigungu = "";
            string dong = "";
            string brand = "";
            if (!IsPostBack)
            {
                //사용자 메뉴 체크
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("login.aspx", false);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["date"])) Sdate = Request.QueryString["date"];
                    if (!String.IsNullOrEmpty(Request.QueryString["sido"])) sido = Request.QueryString["sido"];
                    if (!String.IsNullOrEmpty(Request.QueryString["sigungu"])) sigungu = Request.QueryString["sigungu"];
                    if (!String.IsNullOrEmpty(Request.QueryString["dong"])) dong = Request.QueryString["dong"];
                    if (!String.IsNullOrEmpty(Request.QueryString["brand"])) brand = Request.QueryString["brand"];
                    setDropDown(sido, sigungu, dong, brand);
                    custChart(Sdate);
                    recustChart(Sdate);
                    reChart(Sdate);
                    priceChart(Sdate);
                    grdBind(Sdate, "base_date", "desc");

                    HdnDateS.Value = monthpickerS.Value = Sdate;
                }
            }

            HdnDateS.Value = monthpickerS.Value;

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

        private void setDropDown(string sido, string sigungu, string dong, string brand)
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

            drSido.SelectedValue = sido;
            if(brand != "") drBrend.SelectedValue = drBrend.Items.FindByText(brand).Value;
            
            if (sido != "")
            {
                DataSet dsSigungu = new DataSet();
                query = "select '전체' as sigungu_name,'' as sigungu_code" +
                    " union all" +
                    " select sigungu_name, sigungu_name as sigungu_code from dong_code where sido_name = '" + drSido.SelectedValue + "' group by sigungu_name;";
                dsSigungu = db.SelectDataList(query);

                drSigungu.DataSource = dsSigungu.Tables[0];
                drSigungu.DataTextField = dsSigungu.Tables[0].Columns["sigungu_name"].ToString();
                drSigungu.DataValueField = dsSigungu.Tables[0].Columns["sigungu_code"].ToString();

                drSigungu.DataBind();

                drSigungu.SelectedValue = sigungu;
            }

            if (sigungu != "")
            {
                DataSet dsDong = new DataSet();
                query = "select '전체' as dong_name,'' as dong_code" +
                    " union all" +
                    " select dong_name, dong_name as dong_code from dong_code where sido_name = '" + drSido.SelectedValue + "' and sigungu_name = '" + drSigungu.SelectedValue + "';";
                dsDong = db.SelectDataList(query);

                drDong.DataSource = dsDong.Tables[0];
                drDong.DataTextField = dsDong.Tables[0].Columns["dong_name"].ToString();
                drDong.DataValueField = dsDong.Tables[0].Columns["dong_code"].ToString();

                drDong.DataBind();

                drDong.SelectedValue = dong;
            }
        }

        private void priceChart(string date)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     a.ymd as base_date");
            query.Append("     ,count(b.base_date) as tot_cnt");
            query.Append("     ,(case when sum(price) is null then 0 else sum(price) end) as price");
            query.Append(" from date_ymd a");
            query.Append(" left join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         b.base_date");
            query.Append("         ,b.cust_no");
            query.Append("         ,b.price");
            query.Append("     from order_list_new b");
            query.Append("     inner join order_cust c");
            query.Append("     on b.cust_no = c.cust_no");
            query.Append("     inner join order_cust_addr d");
            query.Append("     on b.cust_no = d.cust_no");
            query.Append("     where c.create_date < b.base_date");
            query.Append("     and b.brand_code like '%" + drBrend.SelectedValue + "%'");
            query.Append("     and d.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and d.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and d.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and d.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append(" ) b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(a.ymd,7) = '" + date + "'");
            query.Append(" group by a.ymd;");
            #endregion

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart
            StringBuilder chart = new StringBuilder();
            chart.Append(" var canvas = document.getElementById('priceChart');");
            chart.Append(" new Chart(canvas, {");
            chart.Append("         type: 'line',");
            chart.Append("         data: {");
            chart.Append("         labels: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["base_date"].ToString() + "',");
            }
            chart.Append("],");
            chart.Append("             datasets: [{");
            chart.Append("         label: '건수',");
            chart.Append("                 fill: false,");
            chart.Append("                 borderColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 backgroundColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 yAxisID: 'A',");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["tot_cnt"].ToString() + ",");
            }
            chart.Append("]");
            chart.Append("             }, {");
            chart.Append("         label: '금액',");
            chart.Append("                 fill: false,");
            chart.Append("                 borderColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 backgroundColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 yAxisID: 'B',");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["price"].ToString() + ",");
            }
            chart.Append("]");
            chart.Append("             }]");
            chart.Append("         },");
            chart.Append("         options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("             scales:");
            chart.Append("         {");
            chart.Append("         yAxes: [{");
            chart.Append("             id: 'A',");
            chart.Append("                     type: 'linear',");
            chart.Append("                     position: 'left',");
            chart.Append("                 }, {");
            chart.Append("             id: 'B',");
            chart.Append("                     type: 'linear',");
            chart.Append("                     position: 'right',");
            chart.Append("                 }]");
            chart.Append("             }");
            chart.Append("     }");
            chart.Append(" });");
            #endregion

            Page.ClientScript.RegisterStartupScript(this.GetType(), "repricechart", chart.ToString(), true);
        }

        private void reChart(string date)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     a.ymd as base_date");
            query.Append("     ,count(b.base_date) as tot_cnt");
            query.Append("     ,(case when sum(price) is null then 0 else sum(price) end) as price");
            query.Append(" from date_ymd a");
            query.Append(" left join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         b.base_date");
            query.Append("         ,b.cust_no");
            query.Append("         ,b.price");
            query.Append("     from order_list_new b");
            query.Append("     inner join order_cust c");
            query.Append("     on b.cust_no = c.cust_no");
            query.Append("     inner join order_cust_addr d");
            query.Append("     on b.cust_no = d.cust_no");
            query.Append("     where c.create_date >= b.base_date");
            query.Append("     and b.brand_code like '%" + drBrend.SelectedValue + "%'");
            query.Append("     and d.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and d.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and d.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and d.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append(" ) b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(a.ymd,7) = '" + date + "'");
            query.Append(" group by a.ymd;");
            #endregion

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart
            StringBuilder chart = new StringBuilder();
            chart.Append(" var canvas = document.getElementById('reChart');");
            chart.Append(" new Chart(canvas, {");
            chart.Append("         type: 'line',");
            chart.Append("         data: {");
            chart.Append("         labels: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["base_date"].ToString() + "',");
            }
            chart.Append("],");
            chart.Append("             datasets: [{");
            chart.Append("         label: '건수',");
            chart.Append("                 fill: false,");
            chart.Append("                 borderColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 backgroundColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 yAxisID: 'A',");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["tot_cnt"].ToString() + ",");
            }
            chart.Append("]");
            chart.Append("             }, {");
            chart.Append("         label: '금액',");
            chart.Append("                 fill: false,");
            chart.Append("                 borderColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 backgroundColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 yAxisID: 'B',");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["price"].ToString() + ",");
            }
            chart.Append("]");
            chart.Append("             }]");
            chart.Append("         },");
            chart.Append("         options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("             scales:");
            chart.Append("         {");
            chart.Append("         yAxes: [{");
            chart.Append("             id: 'A',");
            chart.Append("                     type: 'linear',");
            chart.Append("                     position: 'left',");
            chart.Append("                 }, {");
            chart.Append("             id: 'B',");
            chart.Append("                     type: 'linear',");
            chart.Append("                     position: 'right',");
            chart.Append("                 }]");
            chart.Append("             }");
            chart.Append("     }");
            chart.Append(" });");
            #endregion

            Page.ClientScript.RegisterStartupScript(this.GetType(), "pricechart", chart.ToString(), true);
        }

        private void custChart(string date)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     a.ymd as base_date");
            query.Append("     ,count(b.base_date) as tot_cnt");
            query.Append(" from date_ymd a");
            query.Append(" left join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         b.base_date");
            query.Append("         ,b.cust_no");
            query.Append("     from order_list_new b");
            query.Append("     inner join order_cust c");
            query.Append("     on b.cust_no = c.cust_no");
            query.Append("     inner join order_cust_addr d");
            query.Append("     on b.cust_no = d.cust_no");
            query.Append("     where c.create_date >= b.base_date");
            query.Append("     and b.brand_code like '%" + drBrend.SelectedValue + "%'");
            query.Append("     and d.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and d.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and d.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and d.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append(" ) b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(a.ymd,7) = '"+ date + "'");
            query.Append(" group by a.ymd;");
            #endregion

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart
            StringBuilder chart = new StringBuilder();
            chart.Append(" var ctx = document.getElementById('custChart').getContext('2d');");
            chart.Append(" var mixedChart = new Chart(ctx, {");
            chart.Append("     type: 'bar',");
            chart.Append("     data:");
            chart.Append(" {");
            chart.Append(" datasets: [{");
            chart.Append("     label: '고객수',");
            chart.Append("             borderColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 backgroundColor: 'rgba(64, 198, 90, 1)',");
            chart.Append("                 barPercentage: 1,");
            chart.Append("                 minBarLength: 2,");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["tot_cnt"].ToString() + ",");
            }
            chart.Append("],");
            chart.Append("         }],");
            chart.Append("         labels: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["base_date"].ToString() + "',");
            }
            chart.Append("],");
            chart.Append("     },");
            chart.Append("     options: options");
            chart.Append(" });");

            #endregion

            Page.ClientScript.RegisterStartupScript(this.GetType(), "cust", chart.ToString(), true);
        }

        private void recustChart(string date)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     a.ymd as base_date");
            query.Append("     ,count(b.base_date) as tot_cnt");
            query.Append(" from date_ymd a");
            query.Append(" left join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         b.base_date");
            query.Append("         ,b.cust_no");
            query.Append("     from order_list_new b");
            query.Append("     inner join order_cust c");
            query.Append("     on b.cust_no = c.cust_no");
            query.Append("     inner join order_cust_addr d");
            query.Append("     on b.cust_no = d.cust_no");
            query.Append("     where c.create_date < b.base_date");
            query.Append("     and b.brand_code like '%" + drBrend.SelectedValue + "%'");
            query.Append("     and d.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and d.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and d.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and d.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append(" ) b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(a.ymd,7) = '" + date + "'");
            query.Append(" group by a.ymd;");
            #endregion

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart
            StringBuilder chart = new StringBuilder();
            chart.Append(" var ctx = document.getElementById('recustChart').getContext('2d');");
            chart.Append(" var mixedChart = new Chart(ctx, {");
            chart.Append("     type: 'bar',");
            chart.Append("     data:");
            chart.Append(" {");
            chart.Append(" datasets: [{");
            chart.Append("     label: '신규고객수',");
            chart.Append("             borderColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 backgroundColor: 'rgba(83, 115, 221, 1)',");
            chart.Append("                 barPercentage: 1,");
            chart.Append("                 minBarLength: 2,");
            chart.Append("             data: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append(ds.Tables[0].Rows[i]["tot_cnt"].ToString() + ",");
            }
            chart.Append("],");
            chart.Append("         }],");
            chart.Append("         labels: [ ");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["base_date"].ToString() + "',");
            }
            chart.Append("],");
            chart.Append("     },");
            chart.Append("     options: options");
            chart.Append(" });");

            #endregion

            Page.ClientScript.RegisterStartupScript(this.GetType(), "recust", chart.ToString(), true);
        }

        protected void btnSerch_Click(object sender, EventArgs e)
        {
            this.grdDetail.PageIndex = 0;

            custChart(HdnDateS.Value);
            recustChart(HdnDateS.Value);
            reChart(HdnDateS.Value);
            priceChart(HdnDateS.Value);
            grdBind(HdnDateS.Value, "base_date", "desc");
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

        private void grdBind(string date, string field, string sort)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }

            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("      a.cust_no");
            query.Append("      ,a.base_date");
            query.Append("      ,a.company");
            query.Append("      ,c.comm_name as brand_name");
            query.Append("      ,d.comm_name as product_name");
            query.Append("      ,b.cust_name");
            query.Append("      ,b.tel_no");
            query.Append("      ,b.address");
            query.Append("      ,(select sum(price) from order_list_new where cust_no = a.cust_no group by cust_no) as sumPrice");
            query.Append("      ,(select count(*) from order_list_new where cust_no = a.cust_no group by cust_no) as cnt");
            query.Append(" from order_list_new a");
            query.Append(" inner join order_cust b");
            query.Append(" on a.cust_no = b.cust_no");
            query.Append(" inner join comm_code c");
            query.Append(" on a.brand_code = c.comm_code");
            query.Append(" inner join comm_code d");
            query.Append(" on a.product_code = d.comm_code");
            query.Append(" inner join order_cust_addr e");
            query.Append(" on a.cust_no = e.cust_no");
            query.Append(" where order_no in ");
            query.Append(" 	            (");
            query.Append("                     select");
            query.Append("                         max(order_no)");
            query.Append("                     from order_list_new");
            query.Append("                     where cust_no in (");
            query.Append("                                         select cust_no from order_cust where cust_no in (select cust_no from order_list_new where left(base_date,7) = '"+ date + "')");
            query.Append(" 						             )");
            query.Append(" 		            group by cust_no");
            query.Append("                )");
            query.Append("     and e.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and e.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and e.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and e.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append("     order by "+ field + " " + sort);
            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            grdDetail.DataSource = ds;
            grdDetail.DataBind();
        }

        protected void grdDetail_DataBound(object sender, EventArgs e)
        {
            GridView detail = (GridView)sender;
            for (int i = 0; i < detail.Rows.Count; i++)
            {
                StringBuilder query = new StringBuilder();
                string grdId = grdDetail.DataKeys[i].Value.ToString();
                query.Append(" select");
                query.Append("     a.base_date");
                query.Append("     ,a.company");
                query.Append("     ,b.comm_name as brand_name");
                query.Append("     ,c.comm_name as product_name");
                query.Append("     ,a.price");
                query.Append(" from order_list_new a");
                query.Append(" inner join comm_code b");
                query.Append(" on a.brand_code = b.comm_code");
                query.Append(" inner join comm_code c");
                query.Append(" on a.product_code = c.comm_code");
                query.Append(" where a.cust_no = '"+ grdId + "'");
                query.Append(" order by base_date");

                DataSet ds = new DataSet();
                dashboardDb db = new dashboardDb();
                ds = db.SelectDataList(query.ToString());

                
                GridView grdDetailSub = detail.Rows[i].FindControl("grdDetailSub") as GridView;
                grdDetailSub.DataSource = ds;
                grdDetailSub.DataBind();
            }
        }

        protected void grdExcel_DataBound(object sender, EventArgs e)
        {
            GridView detail = (GridView)sender;
            for (int i = 0; i < detail.Rows.Count; i++)
            {
                StringBuilder query = new StringBuilder();
                string grdId = grdExcel.DataKeys[i].Value.ToString();
                query.Append(" select");
                query.Append("     a.base_date");
                query.Append("     ,a.company");
                query.Append("     ,b.comm_name as brand_name");
                query.Append("     ,c.comm_name as product_name");
                query.Append("     ,a.price");
                query.Append(" from order_list_new a");
                query.Append(" inner join comm_code b");
                query.Append(" on a.brand_code = b.comm_code");
                query.Append(" inner join comm_code c");
                query.Append(" on a.product_code = c.comm_code");
                query.Append(" where a.cust_no = '" + grdId + "'");
                query.Append(" order by base_date");

                DataSet ds = new DataSet();
                dashboardDb db = new dashboardDb();
                ds = db.SelectDataList(query.ToString());


                GridView grdExcelSub = detail.Rows[i].FindControl("grdExcelSub") as GridView;
                grdExcelSub.DataSource = ds;
                grdExcelSub.DataBind();
            }
        }

        protected void grdDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.grdDetail.PageIndex = e.NewPageIndex;

            grdBind(HdnDateS.Value, SortColumn, SortDirection);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string sigungu1 = "";
            string sigungu2 = "";
            for (int i = 0; i < drSigungu.SelectedValue.Split(' ').Length; i++)
            {
                if (i == 0) sigungu1 = drSigungu.SelectedValue.Split(' ')[i];
                else sigungu2 = drSigungu.SelectedValue.Split(' ')[i];
            }

            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("      a.cust_no");
            query.Append("      ,a.base_date");
            query.Append("      ,a.company");
            query.Append("      ,c.comm_name as brand_name");
            query.Append("      ,d.comm_name as product_name");
            query.Append("      ,b.cust_name");
            query.Append("      ,b.tel_no");
            query.Append("      ,b.address");
            query.Append("      ,(select sum(price) from order_list_new where cust_no = a.cust_no group by cust_no) as sumPrice");
            query.Append("      ,(select count(*) from order_list_new where cust_no = a.cust_no group by cust_no) as cnt");
            query.Append(" from order_list_new a");
            query.Append(" inner join order_cust b");
            query.Append(" on a.cust_no = b.cust_no");
            query.Append(" inner join comm_code c");
            query.Append(" on a.brand_code = c.comm_code");
            query.Append(" inner join comm_code d");
            query.Append(" on a.product_code = d.comm_code");
            query.Append(" inner join order_cust_addr e");
            query.Append(" on a.cust_no = e.cust_no");
            query.Append(" where order_no in ");
            query.Append(" 	            (");
            query.Append("                     select");
            query.Append("                         max(order_no)");
            query.Append("                     from order_list_new");
            query.Append("                     where cust_no in (");
            query.Append("                                         select cust_no from order_cust where cust_no in (select cust_no from order_list_new where left(base_date,7) = '" + HdnDateS.Value + "')");
            query.Append(" 						             )");
            query.Append(" 		            group by cust_no");
            query.Append("                )");
            query.Append("     and e.address_1 like '%" + drSido.SelectedValue + "%'");
            query.Append("     and e.address_2 like '%" + sigungu1 + "%'");
            query.Append("     and e.address_3 like '%" + sigungu2 + "%'");
            query.Append("     and e.address_4 like '%" + drDong.SelectedValue + "%'");
            query.Append("     order by base_date desc;");
            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            grdExcel.DataSource = ds;
            grdExcel.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "asdf", "loading('none');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "asdf", "loading('none');", true);

            ExportGridToExcel();
        }

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "고객리스트_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "appllication/vnd.ms-excel";
            Response.CacheControl = "public";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            grdExcel.GridLines = GridLines.Both;
            grdExcel.HeaderStyle.Font.Bold = true;
            grdExcel.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
        }

        protected void grdDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
            SortColumn = e.SortExpression;

            grdBind(HdnDateS.Value, SortColumn, SortDirection);
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