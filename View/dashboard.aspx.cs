using ionpolis.Controller;
using ionpolis.Db;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;

namespace ionpolis.View
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Sdate = DateTime.Now.ToString("yyyy-MM");
            //string Edate = DateTime.Now.ToString("yyyy-MM");

            if (!IsPostBack)
            {
                //사용자 메뉴 체크
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("login.aspx", false);
                }

                HdnSido.Value = "";
                HdnSigungu.Value = "";
                HdnDong.Value = "";

                HdnDateS.Value = Sdate;
                //HdnDateE.Value = Edate;
                setSido(true, "", "", "");
            }

            monthpickerS.Value = HdnDateS.Value;
            //monthpickerE.Value = HdnDateE.Value;

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

        private void setSido(bool postback, string sido, string sigungu, string dong)
        {
            //if (postback)
            //{
            //    DataSet ds = new DataSet();
            //    dashboardDb db = new dashboardDb();
            //    ds = db.SelectDataList("select sigungu_code, sigungu_full_name from sigungu where sigungu_sub_code = 'M';");

            //    string html = "<a href='#' class='dropdown-item' style='font-size:12px' onclick=\"SidodropDownClick('')\">전체</a>";
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        html += "<a href='#' class='dropdown-item' style='font-size:12px' onclick=\"SidodropDownClick('" + ds.Tables[0].Rows[i]["sigungu_full_name"] + "')\">" + ds.Tables[0].Rows[i]["sigungu_full_name"] + "</a>";
            //    }

            //    dropDomainList.InnerHtml = html;
            //}

            SidoChart(postback, sido, sigungu, dong);
            BrandChart(postback, sido, sigungu, dong);
            SigunguChart(postback, sido, sigungu, dong, "");
            ReChart(postback, sido, sigungu, dong);
        }

        private void ReChart(bool postback, string sido, string sigungu, string dong)
        {
            StringBuilder query = new StringBuilder();
            if (sigungu == "")
            {
                query.Append(" select ");
                query.Append("      concat(b.address_1,' ', b.address_2) as address2 ");
                query.Append("     ,count(*) cnt ");
                query.Append(" from order_list_new a ");
                query.Append(" inner join order_cust_addr b ");
                query.Append(" on a.cust_no = b.cust_no ");
                query.Append(" inner join order_cust c ");
                query.Append(" on a.cust_no = c.cust_no ");
                query.Append(" where left(base_date,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
                query.Append(" and c.create_date < a.base_date");
                if (sido != "")
                {
                    query.Append("     and b.address_1 like '%" + sido + "%'");
                }
                if (sigungu != "")
                {
                    query.Append("     and b.address_2 like '%" + sigungu + "%'");
                }
                if (dong != "")
                {
                    query.Append("     and b.address_4 like '%" + dong + "%'");
                }
                query.Append(" group by b.address_1 ,b.address_2 ");
                query.Append(" order by count(*) desc limit 30 ");
            }
            else
            {
                query.Append(" select ");
                query.Append("      b.address_4 as address2 ");
                query.Append("     ,count(*) cnt ");
                query.Append(" from order_list_new a ");
                query.Append(" inner join order_cust_addr b ");
                query.Append(" on a.cust_no = b.cust_no ");
                query.Append(" inner join order_cust c ");
                query.Append(" on a.cust_no = c.cust_no ");
                query.Append(" where left(base_date,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
                query.Append(" and c.create_date < a.base_date");
                if (sido != "")
                {
                    query.Append("     and b.address_1 like '%" + sido + "%'");
                }
                if (sigungu != "")
                {
                    query.Append("     and b.address_2 like '%" + sigungu + "%'");
                }
                if (dong != "")
                {
                    query.Append("     and b.address_4 like '%" + dong + "%'");
                }
                query.Append(" group by b.address_1, b.address_2, b.address_3, b.address_4  ");
                query.Append(" order by count(*) desc limit 30 ");
            }

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart

            StringBuilder chart = new StringBuilder();
            chart.Append(" var ctx = document.getElementById('myChartR').getContext('2d');");
            chart.Append(" var myChart = new Chart(ctx, {");
            chart.Append(" type: 'horizontalBar',");
            chart.Append(" data:");
            chart.Append(" {");
            chart.Append(" labels: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["address2"] + "',");
            }
            chart.Append(" ], ");
            chart.Append(" datasets: [{");
            chart.Append(" label: '");
            chart.Append(sido == "" ? "" : (sido + " " + sigungu + " " + dong));
            chart.Append("',");
            chart.Append(" data: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["cnt"] + "',");
            }
            chart.Append(" ],");
            chart.Append(" borderColor: 'rgba(64, 198, 90, 1)',");
            chart.Append(" backgroundColor: 'rgba(64, 198, 90, 0.5)',");
            chart.Append(" fill: false,");
            chart.Append(" }]");
            chart.Append(" },");
            chart.Append(" options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("                 scales:");
            chart.Append("         {");
            chart.Append("         xAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }],");
            chart.Append("                     yAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }]");
            chart.Append("                 },");
            chart.Append("                 tooltips:");
            chart.Append("         {");
            chart.Append("         enabled: false");
            chart.Append("                 },");
            chart.Append("                 hover:");
            chart.Append("         {");
            chart.Append("         animationDuration: 0");
            chart.Append("                 },");
            chart.Append("                 animation:");
            chart.Append("         {");
            chart.Append("         duration: 1,");
            chart.Append("                 onComplete: function() {");
            chart.Append("                 var chartInstance = this.chart,");
            chart.Append("                     ctx = chartInstance.ctx;");
            chart.Append("                 ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);");
            chart.Append("                 ctx.fillStyle = 'balck';");
            chart.Append("                 ctx.textAlign = 'center';");
            chart.Append("                 ctx.textBaseline = 'bottom';");
            chart.Append(" ");
            chart.Append("                 this.data.datasets.forEach(function(dataset, i) {");
            chart.Append("                     var meta = chartInstance.controller.getDatasetMeta(i);");
            chart.Append("                     meta.data.forEach(function(bar, index) {");
            chart.Append("                         var data = dataset.data[index];");
            chart.Append("                         ctx.fillText(data, bar._model.x, bar._model.y - 5);");
            chart.Append("                     });");
            chart.Append("                 });");
            chart.Append("             }");
            chart.Append("         }");
            chart.Append("     }");
            chart.Append("});");

            #endregion chart

            if (postback)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Rechart", chart.ToString(), true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Rechart", chart.ToString(), true);
            }
        }

        private void SidoChart(bool postback, string sido, string sigungu, string dong)
        {
            #region query

            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     a.ymd as date");
            query.Append("     ,count(b.base_date) as cnt");
            query.Append(" from date_ymd a");
            query.Append(" left");
            query.Append(" join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         base_date");
            query.Append("     from order_list_new a");
            query.Append("     inner join order_cust_addr b");
            query.Append(" on a.cust_no = b.cust_no");
            query.Append("     where b.address_1 like '%"+ sido + "%'");
            query.Append("     and b.address_2 like '%" + sigungu + "%'");
            query.Append("     and b.address_4 like '%" + dong + "%'");
            query.Append(" )b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(ymd,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
            query.Append(" group by a.ymd;");

            #endregion query

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart

            StringBuilder chart = new StringBuilder();
            chart.Append("var ctx = document.getElementById('myChart').getContext('2d');");
            chart.Append("var myChart = new Chart(ctx, {");
            chart.Append("        type: 'bar',");
            chart.Append("        data:");
            chart.Append("    {");
            chart.Append("    labels: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["date"] + "',");
            }
            chart.Append("            ], ");
            chart.Append("            datasets: [{");
            chart.Append("        label: '");
            chart.Append(sido == "" ? "" : (sido + " " + sigungu + " " + dong));
            chart.Append("',");
            chart.Append("                data: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["cnt"] + "',");
            }
            chart.Append("                ],");
            chart.Append("                borderColor: 'rgba(255, 201, 14, 1)',");
            chart.Append("                backgroundColor: 'rgba(255, 201, 14, 0.5)',");
            chart.Append("                fill: true,");
            chart.Append("                lineTension: 0");
            chart.Append("            }]");
            chart.Append("        },");
            chart.Append(" options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("                 scales:");
            chart.Append("         {");
            chart.Append("         xAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }],");
            chart.Append("                     yAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }]");
            chart.Append("                 },");
            chart.Append("                 tooltips:");
            chart.Append("         {");
            chart.Append("         enabled: false");
            chart.Append("                 },");
            chart.Append("                 hover:");
            chart.Append("         {");
            chart.Append("         animationDuration: 0");
            chart.Append("                 },");
            chart.Append("                 animation:");
            chart.Append("         {");
            chart.Append("         duration: 1,");
            chart.Append("                 onComplete: function() {");
            chart.Append("                 var chartInstance = this.chart,");
            chart.Append("                     ctx = chartInstance.ctx;");
            chart.Append("                 ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);");
            chart.Append("                 ctx.fillStyle = 'balck';");
            chart.Append("                 ctx.textAlign = 'center';");
            chart.Append("                 ctx.textBaseline = 'bottom';");
            chart.Append(" ");
            chart.Append("                 this.data.datasets.forEach(function(dataset, i) {");
            chart.Append("                     var meta = chartInstance.controller.getDatasetMeta(i);");
            chart.Append("                     meta.data.forEach(function(bar, index) {");
            chart.Append("                         var data = dataset.data[index];");
            chart.Append("                         ctx.fillText(data, bar._model.x, bar._model.y - 5);");
            chart.Append("                     });");
            chart.Append("                 });");
            chart.Append("             }");
            chart.Append("         }");
            chart.Append("     }");
            chart.Append("});");

            #endregion chart

            if (postback)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Sido", chart.ToString(), true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Sido", chart.ToString(), true);
            }
        }

        private void BrandChart(bool postback, string sido, string sigungu, string dong)
        {
            #region query

            StringBuilder query = new StringBuilder();
            query.Append(" select ");
            query.Append("     b.comm_name ");
            query.Append("     ,cnt ");
            query.Append(" from ");
            query.Append(" ( ");
            query.Append("     select ");
            query.Append("         brand_code ");
            query.Append("         , count(*) as cnt ");
            query.Append("     from order_list_new a ");
            query.Append("     inner join order_cust_addr b ");
            query.Append("     on a.cust_no = b.cust_no ");
            query.Append(" where left(base_date,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
            query.Append("     and b.address_1 like '%" + sido + "%'");
            query.Append("     and b.address_2 like '%" + sigungu + "%'");
            query.Append("     and b.address_4 like '%" + dong + "%'");
            query.Append("     group by brand_code ");
            query.Append(" )a ");
            query.Append(" inner join comm_code b ");
            query.Append(" on a.brand_code = b.comm_code ");
            query.Append(" order by cnt desc; ");

            #endregion query

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart

            StringBuilder chart = new StringBuilder();
            chart.Append(" var ctx = document.getElementById('myChartB').getContext('2d');");
            chart.Append(" var myChart = new Chart(ctx, {");
            chart.Append(" type: 'horizontalBar',");
            chart.Append(" data:");
            chart.Append(" {");
            chart.Append(" labels: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["comm_name"] + "',");
            }
            chart.Append(" ], ");
            chart.Append(" datasets: [{");
            chart.Append(" label: '");
            chart.Append(sido == "" ? "" : (sido + " " + sigungu + " " + dong));
            chart.Append("',");
            chart.Append(" data: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["cnt"] + "',");
            }
            chart.Append(" ],");
            chart.Append(" borderColor: 'rgba(151, 244, 19, 1)',");
            chart.Append(" backgroundColor: 'rgba(151, 244, 19, 0.5)',");
            chart.Append(" fill: false,");
            chart.Append(" }]");
            chart.Append(" },");
            chart.Append(" options:");
            chart.Append("     {");
            chart.Append("    onClick: function(c, i) {");
            chart.Append("            e = i[0];");
            chart.Append("            BrandClick(this.data.labels[e._index]);");
            chart.Append("        },");
            chart.Append("     responsive: true,");
            chart.Append("                 scales:");
            chart.Append("         {");
            chart.Append("         xAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }],");
            chart.Append("                     yAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }]");
            chart.Append("                 },");
            chart.Append("                 tooltips:");
            chart.Append("         {");
            chart.Append("         enabled: false");
            chart.Append("                 },");
            chart.Append("                 hover:");
            chart.Append("         {");
            chart.Append("         onHover: function(e) {");
            chart.Append("             var point = this.getElementAtEvent(e);");
            chart.Append("             if (point.length) e.target.style.cursor = 'pointer';");
            chart.Append("             else e.target.style.cursor = 'default';");
            chart.Append("         },");
            chart.Append("         animationDuration: 0");
            chart.Append("                 },");
            chart.Append("                 animation:");
            chart.Append("         {");
            chart.Append("         duration: 1,");
            chart.Append("                 onComplete: function() {");
            chart.Append("                 var chartInstance = this.chart,");
            chart.Append("                     ctx = chartInstance.ctx;");
            chart.Append("                 ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);");
            chart.Append("                 ctx.fillStyle = 'balck';");
            chart.Append("                 ctx.textAlign = 'center';");
            chart.Append("                 ctx.textBaseline = 'bottom';");
            chart.Append(" ");
            chart.Append("                 this.data.datasets.forEach(function(dataset, i) {");
            chart.Append("                     var meta = chartInstance.controller.getDatasetMeta(i);");
            chart.Append("                     meta.data.forEach(function(bar, index) {");
            chart.Append("                         var data = dataset.data[index];");
            chart.Append("                         ctx.fillText(data, bar._model.x, bar._model.y - 5);");
            chart.Append("                     });");
            chart.Append("                 });");
            chart.Append("             }");
            chart.Append("         }");
            chart.Append("     }");
            chart.Append("});");

            #endregion chart

            if (postback)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Brand", chart.ToString(), true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Brand", chart.ToString(), true);
            }
        }

        private void SigunguChart(bool postback, string sido, string sigungu, string dong, string brand)
        {
            #region query

            StringBuilder query = new StringBuilder();
            if (sigungu == "")
            {
                query.Append(" select ");
                query.Append("      concat(b.address_1,' ', b.address_2) as address2 ");
                query.Append("     ,count(*) cnt ");
                query.Append(" from order_list_new a ");
                query.Append(" inner join order_cust_addr b ");
                query.Append(" on a.cust_no = b.cust_no ");
                if (brand != "")
                {
                    query.Append(" inner join comm_code c ");
                    query.Append("  on a.brand_code = c.comm_code ");
                }
                query.Append(" where left(base_date,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
                if (sido != "")
                {
                    query.Append("     and b.address_1 like '%" + sido + "%'");
                }
                if (sigungu != "")
                {
                    query.Append("     and b.address_2 like '%" + sigungu + "%'");
                }
                if (dong != "")
                {
                    query.Append("     and b.address_4 like '%" + dong + "%'");
                }
                if (brand != "")
                {
                    query.Append(" and c.comm_name like '%"+ brand + "%'");
                }
                query.Append(" group by b.address_1 ,b.address_2 ");
                query.Append(" order by count(*) desc limit 30 ");
            }
            else
            {
                query.Append(" select ");
                query.Append("      b.address_4 as address2 ");
                query.Append("     ,count(*) cnt ");
                query.Append(" from order_list_new a ");
                query.Append(" inner join order_cust_addr b ");
                query.Append(" on a.cust_no = b.cust_no ");
                if (brand != "")
                {
                    query.Append(" inner join comm_code c ");
                    query.Append("  on a.brand_code = c.comm_code ");
                }
                query.Append(" where left(base_date,7) between '" + HdnDateS.Value + "' and '" + HdnDateS.Value + "'");
                if (sido != "")
                {
                    query.Append("     and b.address_1 like '%" + sido + "%'");
                }
                if (sigungu != "")
                {
                    query.Append("     and b.address_2 like '%" + sigungu + "%'");
                }
                if (dong != "")
                {
                    query.Append("     and b.address_4 like '%" + dong + "%'");
                }
                if (brand != "")
                {
                    query.Append(" and c.comm_name like '%"+ brand + "%'");
                }
                query.Append(" group by b.address_1, b.address_2, b.address_3, b.address_4  ");
                query.Append(" order by count(*) desc limit 30 ");
            }
            #endregion query

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            #region chart

            StringBuilder chart = new StringBuilder();
            if (brand == "")
            {
                chart.Append(" var ctx = document.getElementById('myChartG').getContext('2d');");
            }
            else
            {
                chart.Append(" var ctx = document.getElementById('myChartBG').getContext('2d');");
            }
            chart.Append(" var myChart = new Chart(ctx, {");
            chart.Append(" type: 'horizontalBar',");
            chart.Append(" data:");
            chart.Append(" {");
            chart.Append(" labels: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["address2"] + "',");
            }
            chart.Append(" ], ");
            chart.Append(" datasets: [{");
            chart.Append(" label: '");
            if (brand == "")
            {
                chart.Append(sido == "" ? "" : (sido + " " + sigungu + " " + dong));
            }
            else
            {
                chart.Append(brand);
            }
            chart.Append("',");
            chart.Append(" data: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["cnt"] + "',");
            }
            chart.Append(" ],");
            chart.Append(" borderColor: 'rgba(10, 161, 242, 1)',");
            chart.Append(" backgroundColor: 'rgba(10, 161, 242, 0.5)',");
            chart.Append(" fill: false,");
            chart.Append(" }]");
            chart.Append(" },");
            chart.Append(" options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("                 scales:");
            chart.Append("         {");
            chart.Append("         xAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }],");
            chart.Append("                     yAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }]");
            chart.Append("                 },");
            chart.Append("                 tooltips:");
            chart.Append("         {");
            chart.Append("         enabled: false");
            chart.Append("                 },");
            chart.Append("                 hover:");
            chart.Append("         {");
            chart.Append("         animationDuration: 0");
            chart.Append("                 },");
            chart.Append("                 animation:");
            chart.Append("         {");
            chart.Append("         duration: 1,");
            chart.Append("                 onComplete: function() {");
            chart.Append("                 var chartInstance = this.chart,");
            chart.Append("                     ctx = chartInstance.ctx;");
            chart.Append("                 ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);");
            chart.Append("                 ctx.fillStyle = 'balck';");
            chart.Append("                 ctx.textAlign = 'center';");
            chart.Append("                 ctx.textBaseline = 'bottom';");
            chart.Append(" ");
            chart.Append("                 this.data.datasets.forEach(function(dataset, i) {");
            chart.Append("                     var meta = chartInstance.controller.getDatasetMeta(i);");
            chart.Append("                     meta.data.forEach(function(bar, index) {");
            chart.Append("                         var data = dataset.data[index];");
            chart.Append("                         ctx.fillText(data, bar._model.x, bar._model.y - 5);");
            chart.Append("                     });");
            chart.Append("                 });");
            chart.Append("             }");
            chart.Append("         }");
            chart.Append("     }");
            chart.Append("});");

            #endregion chart

            if (brand == "")
            {
                if (postback)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Sigungu", chart.ToString(), true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Sigungu", chart.ToString(), true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "selectGeo", chart.ToString(), true);
            }
            
        }

        protected void btnDropdownS_Click(object sender, EventArgs e)
        {
            setSido(false, HdnSido.Value, HdnSigungu.Value, HdnDong.Value);
        }

        protected void btnSerch_Click(object sender, EventArgs e)
        {
            monthpickerS.Value = HdnDateS.Value;
            setSido(false, HdnSido.Value, HdnSigungu.Value, HdnDong.Value);
        }

        protected void btnMap_Click(object sender, EventArgs e)
        {
            setSido(false, HdnSido.Value, HdnSigungu.Value, HdnDong.Value);
        }

        protected void btnBrand_Click(object sender, EventArgs e)
        {
            selectBrand();
            selectGeo();
        }

        private void selectBrand()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" select");
            query.Append("     ymd as date,");
            query.Append("     count(base_date) as cnt");
            query.Append(" from date_ymd a");
            query.Append(" left join");
            query.Append(" (");
            query.Append("     select");
            query.Append("         base_date");
            query.Append("         ,comm_name");
            query.Append("     from order_list_new a");
            query.Append(" ");
            query.Append("     inner join comm_code b");
            query.Append(" ");
            query.Append("     on a.brand_code = b.comm_code");
            query.Append("     inner join order_cust_addr c");
            query.Append("     on a.cust_no = c.cust_no");
            query.Append("     where c.address_1 like '%" + HdnSido.Value + "%'");
            query.Append("     and c.address_2 like '%" + HdnSigungu.Value + "%'");
            query.Append("     and c.address_4 like '%" + HdnDong.Value + "%'");
            query.Append("     and b.comm_name = '" + HdnBrand.Value + "'");
            query.Append(" ) b");
            query.Append(" on a.ymd = b.base_date");
            query.Append(" where left(a.ymd,7) = '" + HdnDateS.Value + "'");
            query.Append(" group by ymd,b.comm_name;");

            DataSet ds = new DataSet();
            dashboardDb db = new dashboardDb();
            ds = db.SelectDataList(query.ToString());

            StringBuilder chart = new StringBuilder();
            chart.Append("var ctx = document.getElementById('myChartBB').getContext('2d');");
            chart.Append("var myChart = new Chart(ctx, {");
            chart.Append("        type: 'bar',");
            chart.Append("        data:");
            chart.Append("    {");
            chart.Append("    labels: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["date"] + "',");
            }
            chart.Append("            ], ");
            chart.Append("            datasets: [{");
            chart.Append("        label: '");
            chart.Append(HdnBrand.Value);
            chart.Append("',");
            chart.Append("                data: [");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                chart.Append("'" + ds.Tables[0].Rows[i]["cnt"] + "',");
            }
            chart.Append("                ],");
            chart.Append(" borderColor: 'rgba(151, 244, 19, 1)',");
            chart.Append(" backgroundColor: 'rgba(151, 244, 19, 0.5)',");
            chart.Append("                fill: true,");
            chart.Append("                lineTension: 0");
            chart.Append("            }]");
            chart.Append("        },");
            chart.Append(" options:");
            chart.Append("     {");
            chart.Append("     responsive: true,");
            chart.Append("                 scales:");
            chart.Append("         {");
            chart.Append("         xAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }],");
            chart.Append("                     yAxes: [{");
            chart.Append("             stacked: false");
            chart.Append("                     }]");
            chart.Append("                 },");
            chart.Append("                 tooltips:");
            chart.Append("         {");
            chart.Append("         enabled: false");
            chart.Append("                 },");
            chart.Append("                 hover:");
            chart.Append("         {");
            chart.Append("         animationDuration: 0");
            chart.Append("                 },");
            chart.Append("                 animation:");
            chart.Append("         {");
            chart.Append("         duration: 1,");
            chart.Append("                 onComplete: function() {");
            chart.Append("                 var chartInstance = this.chart,");
            chart.Append("                     ctx = chartInstance.ctx;");
            chart.Append("                 ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);");
            chart.Append("                 ctx.fillStyle = 'balck';");
            chart.Append("                 ctx.textAlign = 'center';");
            chart.Append("                 ctx.textBaseline = 'bottom';");
            chart.Append(" ");
            chart.Append("                 this.data.datasets.forEach(function(dataset, i) {");
            chart.Append("                     var meta = chartInstance.controller.getDatasetMeta(i);");
            chart.Append("                     meta.data.forEach(function(bar, index) {");
            chart.Append("                         var data = dataset.data[index];");
            chart.Append("                         ctx.fillText(data, bar._model.x, bar._model.y - 5);");
            chart.Append("                     });");
            chart.Append("                 });");
            chart.Append("             }");
            chart.Append("         }");
            chart.Append("     }");
            chart.Append("});");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "selectBrand", chart.ToString(), true);
        }

        private void selectGeo()
        {
            SigunguChart(false, HdnSido.Value, HdnSigungu.Value, HdnDong.Value, HdnBrand.Value);
        }

        protected void btnCust_Click(object sender, EventArgs e)
        {
            Response.Redirect("cust.aspx?date=" + HdnDateS.Value + "&sido=" + HdnSido.Value + "&sigungu=" + HdnSigungu.Value + "&dong=" + HdnDong.Value + "&brand=" + HdnBrand.Value, false);
        }
    }
}