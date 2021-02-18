using ionpolis.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace ionpolis.View
{
    /// <summary>
    /// WebService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod]
        public sidoModel GetSidoList(string code,string sDate, string eDate)
        {
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select ");
            query.Append("     (select count(*) from order_list_new a inner join order_cust b on a.cust_no = b.cust_no where b.address_1 = ");
            query.Append(" '" + code + "'");
            query.Append(") totCnt");
            query.Append("     ,address2");
            query.Append(" from");
            query.Append(" (select");
            query.Append("     b.address_2 as  address2");
            query.Append("     , count(*) cnt");
            query.Append(" from order_list_new a");
            query.Append(" inner join order_cust b");
            query.Append(" on a.cust_no = b.cust_no");
            query.Append(" where base_date between '" + sDate + "' and '" + eDate + "'");
            if (code != "")
            {
                query.Append("     and b.address_1 = '" + code + "' ");
            }
            query.Append(" group by b.address_2");
            query.Append(" )a order by cnt desc limit 3;");
            #endregion query

            //DataSet ds = new DataSet();
            //dashboardDb db = new dashboardDb();
            //ds = db.SelectDataList(query.ToString());

            //sidoModel model = new sidoModel();
            //model.cnt = "0";
            //model.top1 = "";
            //model.top2 = "";
            //model.top3 = "";
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    DataRow dr = ds.Tables[0].Rows[i];
            //    model.cnt = dr["totCnt"].ToString();
            //    if (i == 0)
            //    {
            //        model.top1 = dr["address2"].ToString();
            //    }
            //    if (i == 1)
            //    {
            //        model.top2 = dr["address2"].ToString();
            //    }
            //    if (i == 2)
            //    {
            //        model.top3 = dr["address2"].ToString();
            //    }
            //}

            //return model;

            return null;
        }

        [WebMethod]
        public sidoModel GetSigunguList(string sido, string code, string sDate, string eDate)
        {
            #region query
            StringBuilder query = new StringBuilder();
            query.Append(" select ");
            query.Append("     (select count(*) from order_list_new a inner join order_cust b on a.cust_no = b.cust_no where b.address_1 = '"+ sido + "' and  b.address_2 = ");
            query.Append(" '" + code + "'");
            query.Append(") totCnt");
            query.Append("     ,address3");
            query.Append(" from");
            query.Append(" (select");
            query.Append("     b.address_3 as address3");
            query.Append("     , count(*) cnt");
            query.Append(" from order_list_new a");
            query.Append(" inner join order_cust b");
            query.Append(" on a.cust_no = b.cust_no");
            query.Append(" where base_date between '" + sDate + "' and '" + eDate + "'");
            if (code != "")
            {
                query.Append("     and b.address_1 = '" + sido + "' "); 
                query.Append("     and b.address_2 = '" + code + "' ");
            }
            query.Append(" group by b.address_3");
            query.Append(" )a order by cnt desc limit 3;");
            #endregion query

            //DataSet ds = new DataSet();
            //dashboardDb db = new dashboardDb();
            //ds = db.SelectDataList(query.ToString());

            //sidoModel model = new sidoModel();
            //model.cnt = "0";
            //model.top1 = "";
            //model.top2 = "";
            //model.top3 = "";
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    DataRow dr = ds.Tables[0].Rows[i];
            //    model.cnt = dr["totCnt"].ToString();
            //    if (i == 0)
            //    {
            //        model.top1 = dr["address3"].ToString();
            //    }
            //    if (i == 1)
            //    {
            //        model.top2 = dr["address3"].ToString();
            //    }
            //    if (i == 2)
            //    {
            //        model.top3 = dr["address3"].ToString();
            //    }
            //}

            //return model;

            return null;
        }

        [WebMethod]
        public string GetDongList(string sido, string sigungu)
        {
            //DataSet ds = new DataSet();
            //dashboardDb db = new dashboardDb();
            //ds = db.SelectDataList("select * from dong_script where sido='" + sido + " " + sigungu + "';");

            //List<jsModel> list = new List<jsModel>();
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    DataRow dr = ds.Tables[0].Rows[i];
            //    jsModel model = new jsModel();
            //    model.sido = dr["sido"].ToString();
            //    model.name = dr["name"].ToString();
            //    model.path = dr["path"].ToString();

            //    list.Add(model);
            //}
            //var serializer = new JavaScriptSerializer();
            //return serializer.Serialize(list);
            return "";
        }
    }

    public class sidoModel
    {
        public string cnt { get; set; }
        public string top1 { get; set; }
        public string top2 { get; set; }
        public string top3 { get; set; }
    }

    [DataContract]
    public class jsModel
    {
        [DataMember]
        public string sido { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string path { get; set; }
    }
}
