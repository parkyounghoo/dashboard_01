using ionpolis.Controller;
using ionpolis.Db;
using ionpolis.Model;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace ionpolis.View
{
    public partial class dataAdd : System.Web.UI.Page
    {
        private string strConn = "Server=15.164.244.81;Database=data_voucher;Uid=parkboss;Pwd=parkboss;Charset=utf8;Connect Timeout=28800;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //사용자 메뉴 체크
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("login.aspx", false);

                    grdTable.Visible = false;
                    btnAdd.Visible = false;
                    upLabel.Visible = false;
                }
            }
            else
            {
                UploadFile(sender, e);
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

        protected void UploadFile(object sender, EventArgs e)
        {
            string textValue = "";

            if (Request.Files.Count != 0)
            {
                ConfigurationManager.AppSettings.Set("filePath", "");

                dropzone.Visible = false;
                grdTable.Visible = true;

                string filePath = "";
                string fileName = "";
                //drag drop 파일 저장
                HttpFileCollection files = Request.Files;
                textValue += "files : " + files;
                foreach (string key in files)
                {
                    HttpPostedFile file = files[key];

                    if (file.FileName.Contains("\\"))
                    {
                        fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\")).Replace("\\", "");
                    }
                    else
                    {
                        fileName = file.FileName;
                    }
                    textValue += "filepath : " + fileName;
                    filePath = Server.MapPath("~/Uploads/");
                    textValue += "filepath : " + filePath;
                    file.SaveAs(filePath + fileName);
                }

                ConfigurationManager.AppSettings.Set("filePath", filePath);
                ConfigurationManager.AppSettings.Set("fileName", fileName);
            }
        }

        protected void ibdrop_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["filePath"];
                string fileName = ConfigurationManager.AppSettings["fileName"];
                string reName = fileName.Split('.')[0] + ".xlsx";
                if (fileName.Split('.')[1] == "csv")
                {
                    Application app = new Application();
                    Workbook wb = app.Workbooks.Open(filePath + fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    wb.SaveAs(filePath + reName, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    wb.Close();
                    app.Quit();
                }

                grdTable.Visible = true;
                btnAdd.Visible = true;
                upLabel.Visible = true;

                int cnt = 0;
                List<order_list> list = new List<order_list>();
                List<order_list> listView = new List<order_list>();
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + (filePath + reName) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                    System.Data.DataTable dtExcelRecords = new System.Data.DataTable();
                    con.Open();
                    System.Data.DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                    cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                    dAdapter.SelectCommand = cmd;
                    dAdapter.Fill(dtExcelRecords);

                    cnt = dtExcelRecords.Rows.Count;
                    for (int i = 1; i < dtExcelRecords.Rows.Count; i++)
                    {
                        DataRow dr = dtExcelRecords.Rows[i];
                        order_list model = new order_list();
                        model.company = dr[0].ToString();
                        model.order_number = dr[1].ToString();
                        model.package_number = dr[2].ToString();
                        model.payment_date = dr[3].ToString();
                        model.product_name = dr[4].ToString();
                        model.product_option = dr[5].ToString();
                        model.amount = dr[6].ToString();
                        model.customer = dr[7].ToString();
                        model.tel = dr[8].ToString();
                        model.address = dr[9].ToString();
                        model.order_memo = dr[10].ToString();
                        model.price = dr[11].ToString();
                        model.state = dr[12].ToString();
                        model.shipping_fee = dr[13].ToString();
                        model.state_func = dr[14].ToString();
                        model.shipping_code = dr[15].ToString();
                        model.shipping_company = dr[16].ToString();
                        model.shipping_number = dr[17].ToString();

                        if (i == 1)
                        {
                            listView.Add(model);
                        }

                        list.Add(model);
                    }
                }

                Session["list"] = list;

                upLabel.InnerText = "※ 총 " + cnt.ToString() + "건의 데이터가 업로드 됩니다. 아래 1건의 데이터 필드 확인 후 배치실행 버튼을 눌러주세요.";
                grdTable.DataSource = listView;
                grdTable.DataBind();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<order_list> list = (List<order_list>)Session["list"];
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                dashboardDb db = new dashboardDb();
                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" call proc_order_list_insert");
                    query.Append(" (");
                    query.Append(" '" + list[i].company.Trim() + "', ");
                    query.Append(" '" + list[i].order_number.Trim() + "', ");
                    query.Append(" '" + list[i].package_number.Trim() + "', ");
                    query.Append(" '" + list[i].payment_date.Trim() + "', ");
                    query.Append(" '" + list[i].product_name.Trim() + "', ");
                    query.Append(" '" + list[i].product_option.Trim() + "', ");
                    query.Append(" " + list[i].amount.Trim() == "" ? "0" : list[i].amount + ", ");
                    query.Append(" '" + list[i].customer.Trim() + "', ");
                    query.Append(" '" + list[i].tel.Trim() + "', ");
                    query.Append(" '" + list[i].address.Trim() + "', ");
                    query.Append(" '" + list[i].order_memo.Trim() + "', ");
                    query.Append(" " + (list[i].price.Trim() == "" ? "0" : list[i].price) + ", ");
                    query.Append(" '" + list[i].state.Trim() + "', ");
                    query.Append(" '" + list[i].shipping_fee.Trim() + "', ");
                    query.Append(" '" + list[i].state_func.Trim() + "', ");
                    query.Append(" '" + list[i].shipping_code.Trim() + "', ");
                    query.Append(" '" + list[i].shipping_company.Trim() + "', ");
                    query.Append(" '" + list[i].shipping_number.Trim() + "', ");
                    query.Append(" '" + date + "' ");
                    query.Append(" );");

                    db.Insert(query.ToString());
                }

                db.Insert("insert into batch_status(member_id,status,create_date) values('"+ Session["USER_ID"] + "', 'S', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')");

                Main_Controller.MessageBox(this, "데이터가 서버에 저장 되었습니다. (백그라운드에서 데이터 전처리가 100건당 1~2 분 소요됩니다.)");

                //Thread t1 = new Thread(new ThreadStart(Function1));
                //t1.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private void Function1()
        {
            setData();
        }

        private void setData()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            List<ORDER_LIST_NEW> list = new List<ORDER_LIST_NEW>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                
                DataSet ds = new DataSet();
                DataSet dsBrand = new DataSet();
                DataSet dsProduct = new DataSet();
                conn.Open();
                string sql = "select * from order_list where create_date = '" + date + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds);

                adpt.SelectCommand.CommandText = "select * from comm_code where comm_gubun = '1000' order by comm_order asc";
                adpt.Fill(dsBrand);

                adpt.SelectCommand.CommandText = "select * from comm_code where comm_gubun = '2000'";
                adpt.Fill(dsProduct);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataSet dsNew = new DataSet();
                    DataRow dr = ds.Tables[0].Rows[i];

                    ORDER_LIST_NEW model = new ORDER_LIST_NEW();
                    model.order_no = dr["no"].ToString();
                    model.base_date = dr["payment_date"].ToString().Replace("/", "-");
                    model.company = dr["company"].ToString();
                    model.brand_code = productCode(dr["product_name"].ToString(), dsBrand, "브랜드");
                    model.product_code = productCode(dr["product_name"].ToString(), dsProduct, "제품");
                    model.product_name = dr["product_name"].ToString();
                    model.price = dr["price"].ToString();

                    adpt.SelectCommand.CommandText = "select * from order_list_new where order_no = " + model.order_no;
                    adpt.Fill(dsNew);
                    StringBuilder query = new StringBuilder();
                    if (dsNew.Tables[0].Rows.Count == 0)
                    {
                        query.Append("insert into order_list_new values (");
                        query.Append(model.order_no);
                        query.Append(",'" + model.base_date + "'");
                        query.Append(",'" + model.company + "'");
                        query.Append(",'" + model.brand_code + "'");
                        query.Append(",'" + model.product_code + "'");
                        query.Append(",'" + model.product_name + "'");
                        query.Append(",'" + model.price + "'");
                        query.Append(",null");
                        query.Append(");");
                    }
                    else
                    {
                        query.Append(" update order_list_new");
                        query.Append(" set");
                        query.Append(" base_date = '" + model.base_date + "'");
                        query.Append(" ,company = '" + model.company + "'");
                        query.Append(" ,brand_code = '" + model.brand_code + "'");
                        query.Append(" ,product_code = '" + model.product_code + "'");
                        query.Append(" ,product_name = '" + model.product_name + "'");
                        query.Append(" ,price = '" + model.price + "'");
                        query.Append(" where order_no = " + model.order_no);
                    }

                    MySqlCommand cmd = new MySqlCommand(query.ToString(), conn);
                    cmd.ExecuteNonQuery();
                }
            }
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(" insert into order_cust(cust_name, tel_no, address, create_date)");
                sb.Append(" select distinct a.customer");
                sb.Append("      , replace(case when a.tel like '050%' then ''");
                sb.Append("             when a.tel like '%--%' then ''");
                sb.Append("             when a.tel like '%000%' then ''");
                sb.Append("             when a.tel like '%*%' then ''");
                sb.Append("             else a.tel end, '-','')");
                sb.Append("      , a.address");
                sb.Append("      , min(date_format(payment_date, '%Y-%m-%d'))");
                sb.Append(" from order_list a");
                sb.Append(" left");
                sb.Append(" join order_cust b");
                sb.Append(" on a.customer = b.cust_name");
                sb.Append(" and replace(case when a.tel like '050%' then ''");                
                sb.Append("        when a.tel like '%--%' then ''");
                sb.Append("        when a.tel like '%000%' then ''");
                sb.Append("        when a.tel like '%*%' then ''");
                sb.Append("             else a.tel end, '-','') = b.tel_no");
                sb.Append(" and a.address = b.address");
                sb.Append(" where payment_date<> ''");
                sb.Append(" and a.address not in ('', '.')");
                sb.Append(" and b.cust_no is null");
                sb.Append(" and a.create_date = left(now(), 10)");
                sb.Append(" group by a.customer");
                sb.Append("      , replace(case when a.tel like '050%' then ''");
                sb.Append("             when a.tel like '%--%' then ''");
                sb.Append("             when a.tel like '%000%' then ''");
                sb.Append("             when a.tel like '%*%' then ''");
                sb.Append("             else a.tel end, '-','')");
                sb.Append("      , a.address");
                sb.Append(" order by 4, 1;");
                MySqlCommand cmdCust = new MySqlCommand(sb.ToString(), conn);
                cmdCust.ExecuteNonQuery();

                sb.Clear();
                sb.Append(" update order_list_new a");
                sb.Append(" join(select a.no as order_no");
                sb.Append("      , b.cust_no");
                sb.Append("      , b.cust_name");
                sb.Append(" from(select * from order_list where create_date = left(now(), 10)) a");
                sb.Append(" join order_cust b");
                sb.Append(" on a.customer = b.cust_name");
                sb.Append(" and replace(case when a.tel like '050%' then ''");
                sb.Append("             when a.tel like '%--%' then ''");
                sb.Append("             when a.tel like '%000%' then ''");
                sb.Append("             when a.tel like '%*%' then ''");
                sb.Append("             else a.tel end, '-','') = b.tel_no");
                sb.Append(" and a.address = b.address) b");
                sb.Append(" on a.order_no = b.order_no");
                sb.Append(" set a.cust_no = b.cust_no");
                sb.Append(" where a.cust_no is null or a.cust_no = '' ");

                cmdCust = new MySqlCommand(sb.ToString(), conn);
                cmdCust.ExecuteNonQuery();

                sigungu(date);
                dong2();
                dong3();

                MySqlCommand custUpdate = new MySqlCommand("update order_cust_addr set address_4 = address_3, address_3 = '' where ltrim(address_4) = ''", conn);
                custUpdate.ExecuteNonQuery();

                MySqlCommand levelUpdate = new MySqlCommand("delete from order_cust_addr where address_level = '';", conn);
                levelUpdate.ExecuteNonQuery();
            }
        }

        private void sigungu(string date)
        {
            DataSet ds = new DataSet();
            DataSet dsSido = new DataSet();
            DataSet dsSigungu = new DataSet();
            DataSet dsAddress3 = new DataSet();
            DataSet dsAddress4 = new DataSet();
            List<stringModel> list = new List<stringModel>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                //string sql3 = "select c.cust_no ,c.address from order_list a inner join order_list_new b on a.no = b.order_no inner join order_cust c on b.cust_no = c.cust_no where a.create_date ='"+ date + "';";
                string sql3 = "select c.cust_no ,c.address from order_list a  inner join order_list_new b  on a.no = b.order_no  inner join order_cust c  on b.cust_no = c.cust_no  left join order_cust_addr d on c.cust_no = d.cust_no where a.create_date ='" + date + "' and d.cust_no is null;";
                MySqlDataAdapter adpt3 = new MySqlDataAdapter(sql3, conn);
                adpt3.Fill(ds, "Tab1");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    stringModel model = new stringModel();
                    model.no = ds.Tables[0].Rows[i]["cust_no"].ToString();

                    string txt = ds.Tables[0].Rows[i]["address"].ToString().Replace(" ", "");

                    if (txt.Contains("청원군"))
                    {
                        txt = txt.Replace("청원군", "청주시청원구");
                    }

                    if (txt.Length >= 7)
                    {
                        //MySqlDataAdapter 클래스를 이용하여
                        //비연결 모드로 데이타 가져오기
                        dsSido.Clear();
                        string sql = "SELECT *  FROM sigungu where sigungu_sub_code = 'M'";
                        MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                        adpt.Fill(dsSido, "Tab1");

                        string sidoName = "";
                        string sigunguNameF = "";
                        string sigunguNameS = "";
                        int sidoOrder = 0;
                        for (int j = 0; j < dsSido.Tables[0].Rows.Count; j++)
                        {
                            DataRow dr = dsSido.Tables[0].Rows[j];
                            if (txt.Substring(0, 3).Contains(dr["sigungu_full_name"].ToString().Substring(0, 3)))
                            {
                                txt = txt.Replace(dr["sigungu_full_name"].ToString(), "");
                                model.sido = dr["sigungu_full_name"].ToString();
                                sidoName = dr["sigungu_full_name"].ToString();
                                sidoOrder = int.Parse(dr["sigungu_order"].ToString());
                                model.address_level = "1";
                                break;
                            }

                            if (txt.Substring(0, 2).Contains(dr["sigungu_name"].ToString().Substring(0, 2)))
                            {
                                string txtJ = "";
                                if (txt.Contains("제주시"))
                                {
                                    txtJ = "제주";
                                    txt = txtJ + txt.Replace(dr["sigungu_name"].ToString(), "");
                                }
                                else
                                {
                                    txt = txt.Replace(dr["sigungu_name"].ToString(), "");
                                }
                                model.sido = dr["sigungu_full_name"].ToString();
                                sidoName = dr["sigungu_full_name"].ToString();
                                sidoOrder = int.Parse(dr["sigungu_order"].ToString());
                                model.address_level = "1";

                                break;
                            }
                        }

                        if (sidoName == "부산광역시")
                        {
                            if (!txt.Contains("부산진구"))
                            {
                                if (txt.Contains("진구"))
                                {
                                    txt = txt.Replace("진구", "부산진구");
                                }

                            }
                        }

                        model.order = sidoOrder;

                        if (sidoName == "인천광역시")
                        {
                            if (txt.Contains("남구"))
                            {
                                txt = txt.Replace("남구", "미추홀구");
                            }
                        }

                        if (sidoOrder > 8)
                        {
                            dsSigungu.Clear();
                            string sql2 = "select sido_name, sigungu_name from dong_code where sido_name = '" + sidoName + "' group by sido_name, sigungu_name;";
                            MySqlDataAdapter adpt2 = new MySqlDataAdapter(sql2, conn);
                            adpt2.Fill(dsSigungu, "Tab1");

                            for (int j = 0; j < dsSigungu.Tables[0].Rows.Count; j++)
                            {
                                string[] sigunguName = dsSigungu.Tables[0].Rows[j]["sigungu_name"].ToString().Split(' ');

                                if (txt.Substring(0, 5).Contains(sigunguName[0]))
                                {
                                    txt = sigunguName[0].Length == 0 ? txt : txt.Replace(sigunguName[0], "");
                                    model.sigungu = sigunguName[0];
                                    sigunguNameF = sigunguName[0];
                                    model.address_level = "2";
                                    break;
                                }
                            }

                            dsAddress3.Clear();
                            string sql4 = "select sido_name, sigungu_name from dong_code where sido_name = '" + sidoName + "' group by sido_name, sigungu_name;";
                            MySqlDataAdapter adpt4 = new MySqlDataAdapter(sql4, conn);
                            adpt4.Fill(dsAddress3, "Tab1");

                            for (int j = 0; j < dsAddress3.Tables[0].Rows.Count; j++)
                            {
                                string[] sigunguName = dsSigungu.Tables[0].Rows[j]["sigungu_name"].ToString().Split(' ');
                                if (sigunguName.Length == 2)
                                {
                                    if (txt.Substring(0, 5).Contains(sigunguName[1]))
                                    {
                                        txt = txt.Replace(sigunguName[1], "");
                                        model.address_3 = sigunguName[1];
                                        sigunguNameS = sigunguName[1];
                                        model.address_level = "3";
                                        break;
                                    }
                                }
                            }

                            model.mod = txt;
                        }
                        else
                        {
                            dsSigungu.Clear();
                            string sql2 = "select sido_name, sigungu_name from dong_code where sido_name = '" + sidoName + "' group by sido_name, sigungu_name;";
                            MySqlDataAdapter adpt2 = new MySqlDataAdapter(sql2, conn);
                            adpt2.Fill(dsSigungu, "Tab1");

                            for (int j = 0; j < dsSigungu.Tables[0].Rows.Count; j++)
                            {
                                string[] sigunguName = dsSigungu.Tables[0].Rows[j]["sigungu_name"].ToString().Split(' ');

                                if (txt.Substring(0, 5).Contains(sigunguName[0]))
                                {
                                    txt = sigunguName[0].Length == 0 ? txt : txt.Replace(sigunguName[0], "");
                                    model.sigungu = sigunguName[0];
                                    sigunguNameF = sigunguName[0];
                                    model.address_level = "2";
                                    model.mod = txt;
                                    break;
                                }
                            }
                        }

                        dsAddress4.Clear();
                        string sigunguFullName = "";
                        if (sigunguNameS == "")
                        {
                            sigunguFullName = sigunguNameF;
                        }
                        else
                        {
                            sigunguFullName = (sigunguNameF + " " + sigunguNameS);
                        }
                        string sqladdress4 = "select sido_name, sigungu_name, dong_name from dong_code where sido_name = '" + sidoName + "' and sigungu_name = '" + sigunguFullName + "' group by sido_name, sigungu_name,dong_name;";
                        MySqlDataAdapter adptaddress = new MySqlDataAdapter(sqladdress4, conn);
                        adptaddress.Fill(dsAddress4, "Tab1");

                        for (int j = 0; j < dsAddress4.Tables[0].Rows.Count; j++)
                        {
                            string dongName = dsAddress4.Tables[0].Rows[j]["dong_name"].ToString();

                            if (txt.Substring(0, 6).Contains(dongName))
                            {
                                txt = txt.Replace(dongName, "");
                                if (model.address_3 == null)
                                {
                                    model.address_3 = dongName;
                                    model.address_level = "3";
                                }
                                else
                                {
                                    model.address_4 = dongName;
                                    model.address_level = "4";
                                }

                                model.mod = txt;
                                break;
                            }
                        }

                        if (model.mod == null)
                        {
                            model.mod = txt;
                        }

                        list.Add(model);
                    }
                    else
                    {
                        model.sido = txt;
                        model.mod = "";
                    }
                }

                conn.Open();
                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("call  proc_cust_addr_no (" + list[i].no + ",'" + list[i].sido + "' , ");
                    if (list[i].sigungu == null)
                    {
                        query.Append(" '" + list[i].mod + "', '','','" + list[i].address_level + "' ");
                    }
                    else
                    {
                        query.Append(" '" + list[i].sigungu + "',");

                        if (list[i].address_3 == null)
                        {
                            query.Append(" '" + list[i].mod + "', '', '" + list[i].address_level + "' ");
                        }
                        else
                        {
                            query.Append(" '" + list[i].address_3 + "',");

                            if (list[i].address_4 == null)
                            {
                                query.Append(" '" + list[i].mod + "','" + list[i].address_level + "' ");
                            }
                            else
                            {
                                query.Append(" '" + list[i].address_4 + "','" + list[i].address_level + "' ");
                            }
                        }
                    }

                    query.Append(" )");
                    MySqlCommand cmd = new MySqlCommand(query.ToString(), conn);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void dong2()
        {
            DataSet ds = new DataSet();
            List<stringModel> list = new List<stringModel>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();

                //Level 2 (로)
                string sql = "select cust_no, address_1,address_2,address_3,INSTR(address_3, '로') as address_level from order_cust_addr where address_level = 2";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "Tab1");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    DataSet dsDongF = new DataSet();
                    StringBuilder sbF = new StringBuilder();
                    sbF.Append(" select");
                    sbF.Append("     detail_name");
                    sbF.Append(" from new_doro a");
                    sbF.Append(" join new_addr b");
                    sbF.Append(" on a.doro_code = b.doro_code");
                    sbF.Append(" and a.doro_detail = b.doro_detail");
                    sbF.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                    sbF.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                    sbF.Append(" group by detail_name;");
                    adpt = new MySqlDataAdapter(sbF.ToString(), conn);
                    adpt.Fill(dsDongF, "Tab1");

                    string dongF = "";
                    for (int j = 0; j < dsDongF.Tables[0].Rows.Count; j++)
                    {
                        if (dr["address_3"].ToString().Contains(dsDongF.Tables[0].Rows[j]["detail_name"].ToString()))
                        {
                            dongF = dsDongF.Tables[0].Rows[j]["detail_name"].ToString();
                        }
                    }

                    if (dongF == "")
                    {
                        if (dr["address_level"].ToString() != "0")
                        {
                            string address = dr["address_3"].ToString();
                            string doro_name = address.Substring(0, int.Parse(dr["address_level"].ToString()));

                            address = address.Replace(doro_name, "");
                            string build_m_no = "";
                            int chknum = 0;
                            bool firstchk = false;

                            //숫자 찾기
                            foreach (char c in address)
                            {
                                if (int.TryParse(c.ToString(), out chknum))
                                {
                                    build_m_no += chknum.ToString();
                                    firstchk = true;
                                }
                                else
                                {
                                    if (firstchk)
                                    {
                                        if (c == '길')
                                        {
                                            doro_name = doro_name + build_m_no + c;
                                            build_m_no = "";
                                        }

                                        break;
                                    }
                                }
                            }

                            DataSet dsDong = new DataSet();
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" select");
                            sb.Append("     detail_name");
                            sb.Append(" from new_doro a");
                            sb.Append(" join new_addr b");
                            sb.Append(" on a.doro_code = b.doro_code");
                            sb.Append(" and a.doro_detail = b.doro_detail");
                            sb.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                            sb.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                            sb.Append(" and doro_name like '%" + doro_name + "%'");
                            sb.Append(" and build_m_no like '%" + build_m_no + "%'");
                            sb.Append(" group by detail_name;");
                            adpt = new MySqlDataAdapter(sb.ToString(), conn);
                            adpt.Fill(dsDong, "Tab1");

                            if (dsDong.Tables[0].Rows.Count != 0)
                            {
                                sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsDong.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                MySqlCommand cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string RoF = "";
                                DataSet dsRoF = new DataSet();
                                StringBuilder sbRoF = new StringBuilder();
                                sbRoF.Append(" select");
                                sbRoF.Append("     replace(doro_name, '번길','') as doro_name");
                                sbRoF.Append(" from new_doro a");
                                sbRoF.Append(" join new_addr b");
                                sbRoF.Append(" on a.doro_code = b.doro_code");
                                sbRoF.Append(" and a.doro_detail = b.doro_detail");
                                sbRoF.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                sbRoF.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                sbRoF.Append(" group by doro_name order by CHAR_LENGTH(doro_name) desc, doro_name desc;");
                                adpt = new MySqlDataAdapter(sbRoF.ToString(), conn);
                                adpt.Fill(dsRoF, "Tab1");

                                for (int j = 0; j < dsRoF.Tables[0].Rows.Count; j++)
                                {
                                    if (dr["address_3"].ToString().Contains(dsRoF.Tables[0].Rows[j]["doro_name"].ToString()))
                                    {
                                        RoF = dsRoF.Tables[0].Rows[j]["doro_name"].ToString();

                                        break;
                                    }
                                }

                                if (RoF != "")
                                {
                                    DataSet dsRoE = new DataSet();
                                    StringBuilder sbRoE = new StringBuilder();
                                    sbRoE.Append(" select");
                                    sbRoE.Append("     detail_name");
                                    sbRoE.Append(" from new_doro a");
                                    sbRoE.Append(" join new_addr b");
                                    sbRoE.Append(" on a.doro_code = b.doro_code");
                                    sbRoE.Append(" and a.doro_detail = b.doro_detail");
                                    sbRoE.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                    sbRoE.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                    sbRoE.Append(" and doro_name = '" + RoF + "'");
                                    sbRoE.Append(" group by detail_name;");
                                    adpt = new MySqlDataAdapter(sbRoE.ToString(), conn);
                                    adpt.Fill(dsRoE, "Tab1");

                                    if (dsRoE.Tables[0].Rows.Count == 0)
                                    {
                                        DataSet dsRoL = new DataSet();
                                        StringBuilder sbRoL = new StringBuilder();
                                        sbRoL.Append(" select");
                                        sbRoL.Append("     detail_name");
                                        sbRoL.Append(" from new_doro a");
                                        sbRoL.Append(" join new_addr b");
                                        sbRoL.Append(" on a.doro_code = b.doro_code");
                                        sbRoL.Append(" and a.doro_detail = b.doro_detail");
                                        sbRoL.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                        sbRoL.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                        sbRoL.Append(" and doro_name = '" + doro_name + "'");
                                        sbRoL.Append(" group by detail_name;");
                                        adpt = new MySqlDataAdapter(sbRoL.ToString(), conn);
                                        adpt.Fill(dsRoL, "Tab1");

                                        if (dsRoL.Tables[0].Rows.Count == 0)
                                        {
                                            DataSet dsRoLD = new DataSet();
                                            StringBuilder sbRoLD = new StringBuilder();
                                            sbRoLD.Append(" select* from");
                                            sbRoLD.Append(" (");
                                            sbRoLD.Append("  select");
                                            sbRoLD.Append("     left(doro_name, LOCATE('로',doro_name)) as doro_name");
                                            sbRoLD.Append(" from new_doro a");
                                            sbRoLD.Append(" join new_addr b");
                                            sbRoLD.Append(" on a.doro_code = b.doro_code");
                                            sbRoLD.Append(" and a.doro_detail = b.doro_detail");
                                            sbRoLD.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                            sbRoLD.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                            sbRoLD.Append(" ) a group by a.doro_name;");

                                            adpt = new MySqlDataAdapter(sbRoLD.ToString(), conn);
                                            adpt.Fill(dsRoLD, "Tab1");
                                            for (int j = 0; j < dsRoLD.Tables[0].Rows.Count; j++)
                                            {
                                                string doro = dsRoLD.Tables[0].Rows[j]["doro_name"].ToString();
                                                if (doro_name.Contains(doro))
                                                {
                                                    doro_name = doro;
                                                }
                                            }
                                        }
                                        sbRoL.Clear();
                                        DataSet dsRoLL = new DataSet();
                                        sbRoL.Append(" select");
                                        sbRoL.Append("     detail_name");
                                        sbRoL.Append(" from new_doro a");
                                        sbRoL.Append(" join new_addr b");
                                        sbRoL.Append(" on a.doro_code = b.doro_code");
                                        sbRoL.Append(" and a.doro_detail = b.doro_detail");
                                        sbRoL.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                        sbRoL.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                        sbRoL.Append(" and doro_name like '%" + doro_name + "%'");
                                        sbRoL.Append(" group by detail_name  limit 1;");
                                        adpt = new MySqlDataAdapter(sbRoL.ToString(), conn);
                                        adpt.Fill(dsRoLL, "Tab1");

                                        sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsRoLL.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsRoE.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataSet ds2 = new DataSet();
                            string sql2 = "select address_1,address_2,address_3,INSTR(address_3, '길') as address_level from order_cust_addr where cust_no =" + dr["cust_no"];
                            MySqlDataAdapter adpt2 = new MySqlDataAdapter(sql2, conn);
                            adpt2.Fill(ds2, "Tab1");

                            if (ds2.Tables[0].Rows[0]["address_level"].ToString() != "0")
                            {
                                string address2 = ds2.Tables[0].Rows[0]["address_3"].ToString();
                                string doro_name = address2.Substring(0, int.Parse(ds2.Tables[0].Rows[0]["address_level"].ToString()));

                                address2 = address2.Replace(doro_name, "");
                                string build_m_no = "";
                                int chknum2 = 0;
                                bool firstchk = false;

                                //숫자 찾기
                                foreach (char c in address2)
                                {
                                    if (int.TryParse(c.ToString(), out chknum2))
                                    {
                                        build_m_no += chknum2.ToString();
                                        firstchk = true;
                                    }
                                    else
                                    {
                                        if (firstchk)
                                        {
                                            if (c == '길')
                                            {
                                                doro_name = doro_name + build_m_no + c;
                                                build_m_no = "";
                                            }

                                            break;
                                        }
                                    }
                                }

                                DataSet dsDong = new DataSet();
                                StringBuilder sb = new StringBuilder();
                                sb.Append(" select");
                                sb.Append("     detail_name");
                                sb.Append(" from new_doro a");
                                sb.Append(" join new_addr b");
                                sb.Append(" on a.doro_code = b.doro_code");
                                sb.Append(" and a.doro_detail = b.doro_detail");
                                sb.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                sb.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                sb.Append(" and doro_name like '%" + doro_name + "%'");
                                sb.Append(" and build_m_no like '%" + build_m_no + "%'");
                                sb.Append(" group by detail_name;");
                                adpt = new MySqlDataAdapter(sb.ToString(), conn);
                                adpt.Fill(dsDong, "Tab1");

                                if (dsDong.Tables[0].Rows.Count != 0)
                                {
                                    sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsDong.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                    MySqlCommand cmd2 = new MySqlCommand(sql, conn);
                                    cmd2.ExecuteNonQuery();
                                }
                                else
                                {
                                    string RoF = "";
                                    DataSet dsRoF = new DataSet();
                                    StringBuilder sbRoF = new StringBuilder();
                                    sbRoF.Append(" select");
                                    sbRoF.Append("     replace(doro_name, '번길','') as doro_name");
                                    sbRoF.Append(" from new_doro a");
                                    sbRoF.Append(" join new_addr b");
                                    sbRoF.Append(" on a.doro_code = b.doro_code");
                                    sbRoF.Append(" and a.doro_detail = b.doro_detail");
                                    sbRoF.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                    sbRoF.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                    sbRoF.Append(" group by doro_name order by CHAR_LENGTH(doro_name) desc, doro_name desc;");
                                    adpt = new MySqlDataAdapter(sbRoF.ToString(), conn);
                                    adpt.Fill(dsRoF, "Tab1");

                                    for (int j = 0; j < dsRoF.Tables[0].Rows.Count; j++)
                                    {
                                        if (dr["address_3"].ToString().Contains(dsRoF.Tables[0].Rows[j]["doro_name"].ToString()))
                                        {
                                            RoF = dsRoF.Tables[0].Rows[j]["doro_name"].ToString();

                                            break;
                                        }
                                    }

                                    if (RoF != "")
                                    {
                                        DataSet dsRoE = new DataSet();
                                        StringBuilder sbRoE = new StringBuilder();
                                        sbRoE.Append(" select");
                                        sbRoE.Append("     detail_name");
                                        sbRoE.Append(" from new_doro a");
                                        sbRoE.Append(" join new_addr b");
                                        sbRoE.Append(" on a.doro_code = b.doro_code");
                                        sbRoE.Append(" and a.doro_detail = b.doro_detail");
                                        sbRoE.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                        sbRoE.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                        sbRoE.Append(" and doro_name = '" + RoF + "'");
                                        sbRoE.Append(" group by detail_name;");
                                        adpt = new MySqlDataAdapter(sbRoE.ToString(), conn);
                                        adpt.Fill(dsRoE, "Tab1");

                                        if (dsRoE.Tables[0].Rows.Count == 0)
                                        {
                                            DataSet dsRoL = new DataSet();
                                            StringBuilder sbRoL = new StringBuilder();
                                            sbRoL.Append(" select");
                                            sbRoL.Append("     detail_name");
                                            sbRoL.Append(" from new_doro a");
                                            sbRoL.Append(" join new_addr b");
                                            sbRoL.Append(" on a.doro_code = b.doro_code");
                                            sbRoL.Append(" and a.doro_detail = b.doro_detail");
                                            sbRoL.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                            sbRoL.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                            sbRoL.Append(" and doro_name = '" + doro_name + "'");
                                            sbRoL.Append(" group by detail_name;");
                                            adpt = new MySqlDataAdapter(sbRoL.ToString(), conn);
                                            adpt.Fill(dsRoL, "Tab1");

                                            if (dsRoL.Tables[0].Rows.Count == 0)
                                            {
                                                DataSet dsRoLD = new DataSet();
                                                StringBuilder sbRoLD = new StringBuilder();
                                                sbRoLD.Append(" select* from");
                                                sbRoLD.Append(" (");
                                                sbRoLD.Append("  select");
                                                sbRoLD.Append("     left(doro_name, LOCATE('로',doro_name)) as doro_name");
                                                sbRoLD.Append(" from new_doro a");
                                                sbRoLD.Append(" join new_addr b");
                                                sbRoLD.Append(" on a.doro_code = b.doro_code");
                                                sbRoLD.Append(" and a.doro_detail = b.doro_detail");
                                                sbRoLD.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                                sbRoLD.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                                sbRoLD.Append(" ) a group by a.doro_name;");

                                                adpt = new MySqlDataAdapter(sbRoLD.ToString(), conn);
                                                adpt.Fill(dsRoLD, "Tab1");
                                                for (int j = 0; j < dsRoLD.Tables[0].Rows.Count; j++)
                                                {
                                                    string doro = dsRoLD.Tables[0].Rows[j]["doro_name"].ToString();
                                                    if (doro_name.Contains(doro))
                                                    {
                                                        doro_name = doro;
                                                    }
                                                }
                                            }
                                            sbRoL.Clear();
                                            DataSet dsRoLL = new DataSet();
                                            sbRoL.Append(" select");
                                            sbRoL.Append("     detail_name");
                                            sbRoL.Append(" from new_doro a");
                                            sbRoL.Append(" join new_addr b");
                                            sbRoL.Append(" on a.doro_code = b.doro_code");
                                            sbRoL.Append(" and a.doro_detail = b.doro_detail");
                                            sbRoL.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                            sbRoL.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                                            sbRoL.Append(" and doro_name like '%" + doro_name + "%'");
                                            sbRoL.Append(" group by detail_name  limit 1;");
                                            adpt = new MySqlDataAdapter(sbRoL.ToString(), conn);
                                            adpt.Fill(dsRoLL, "Tab1");

                                            sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsRoLL.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                            MySqlCommand cmd2 = new MySqlCommand(sql, conn);
                                            cmd2.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            sql = "update order_cust_addr set address_level = 3, address_3 = '" + dsRoE.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                            MySqlCommand cmd2 = new MySqlCommand(sql, conn);
                                            cmd2.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string address = dr["address_3"].ToString();
                                string dong_name = address.Substring(0, address.IndexOf('동') + 1);
                                int chknum = 0;
                                int cnt = 0;
                                string dong_name_new = "";
                                foreach (char c in dong_name)
                                {
                                    if (!int.TryParse(c.ToString(), out chknum))
                                    {
                                        dong_name_new += c;
                                    }

                                    cnt++;
                                }

                                sql = "update order_cust_addr set address_level = 3, address_3 = '" + (dong_name_new == "" ? dr["address_3"] : dong_name_new) + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                                MySqlCommand cmd2 = new MySqlCommand(sql, conn);
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        sql = "update order_cust_addr set address_level = 3, address_3 = '" + dongF + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void dong3()
        {
            DataSet ds = new DataSet();
            List<stringModel> list = new List<stringModel>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();

                //Level 2 (로)
                string sql = "select address_1,address_2,address_3,address_4,INSTR(address_4, '로') as address_level from order_cust_addr where address_level = 3";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "Tab1");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    DataSet dsSearch = new DataSet();
                    StringBuilder sbSearch = new StringBuilder();
                    sbSearch.Append(" select");
                    sbSearch.Append("     detail_name");
                    sbSearch.Append(" from new_doro a");
                    sbSearch.Append(" join new_addr b");
                    sbSearch.Append(" on a.doro_code = b.doro_code");
                    sbSearch.Append(" and a.doro_detail = b.doro_detail");
                    sbSearch.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                    sbSearch.Append(" and sigungu_name = '" + dr["address_2"].ToString() + "'");
                    sbSearch.Append(" and detail_name = '" + dr["address_3"].ToString() + "'");
                    sbSearch.Append(" group by detail_name;");
                    adpt = new MySqlDataAdapter(sbSearch.ToString(), conn);
                    adpt.Fill(dsSearch, "Tab1");

                    if (dsSearch.Tables[0].Rows.Count == 0)
                    {
                        DataSet dsDongF = new DataSet();
                        StringBuilder sbF = new StringBuilder();
                        sbF.Append(" select");
                        sbF.Append("     detail_name");
                        sbF.Append(" from new_doro a");
                        sbF.Append(" join new_addr b");
                        sbF.Append(" on a.doro_code = b.doro_code");
                        sbF.Append(" and a.doro_detail = b.doro_detail");
                        sbF.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                        sbF.Append(" and sigungu_name = '" + (dr["address_2"].ToString() + " " + dr["address_3"].ToString()) + "'");
                        sbF.Append(" group by detail_name;");
                        adpt = new MySqlDataAdapter(sbF.ToString(), conn);
                        adpt.Fill(dsDongF, "Tab1");

                        string dongF = "";
                        for (int j = 0; j < dsDongF.Tables[0].Rows.Count; j++)
                        {
                            if (dr["address_4"].ToString().Contains(dsDongF.Tables[0].Rows[j]["detail_name"].ToString()))
                            {
                                dongF = dsDongF.Tables[0].Rows[j]["detail_name"].ToString();
                            }
                        }

                        if (dongF == "")
                        {
                            if (dr["address_level"].ToString() != "0")
                            {
                                string address = dr["address_4"].ToString();
                                string doro_name = address.Substring(0, int.Parse(dr["address_level"].ToString()));
                                address = address.Replace(doro_name, "");
                                string build_m_no = "";
                                int chknum = 0;
                                bool firstchk = false;

                                //숫자 찾기
                                foreach (char c in address)
                                {
                                    if (int.TryParse(c.ToString(), out chknum))
                                    {
                                        build_m_no += chknum.ToString();
                                        firstchk = true;
                                    }
                                    else
                                    {
                                        if (firstchk)
                                        {
                                            if (c == '길')
                                            {
                                                doro_name = doro_name + build_m_no + c;
                                                build_m_no = "";
                                            }

                                            break;
                                        }
                                    }
                                }

                                DataSet dsDong = new DataSet();
                                StringBuilder sb = new StringBuilder();
                                sb.Append(" select");
                                sb.Append("     detail_name");
                                sb.Append(" from new_doro a");
                                sb.Append(" join new_addr b");
                                sb.Append(" on a.doro_code = b.doro_code");
                                sb.Append(" and a.doro_detail = b.doro_detail");
                                sb.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                sb.Append(" and sigungu_name = '" + (dr["address_2"].ToString() + " " + dr["address_3"].ToString()) + "'");
                                sb.Append(" and doro_name like '%" + doro_name + "%'");
                                sb.Append(" and build_m_no like '%" + build_m_no + "%'");
                                sb.Append(" group by detail_name;");
                                adpt = new MySqlDataAdapter(sb.ToString(), conn);
                                adpt.Fill(dsDong, "Tab1");

                                if (dsDong.Tables[0].Rows.Count != 0)
                                {
                                    sql = "update order_cust_addr set address_level = 0, address_4 = '" + dsDong.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    string RoF = "";
                                    DataSet dsRoF = new DataSet();
                                    StringBuilder sbRoF = new StringBuilder();
                                    sbRoF.Append(" select");
                                    sbRoF.Append("     replace(doro_name, '번길','') as doro_name");
                                    sbRoF.Append(" from new_doro a");
                                    sbRoF.Append(" join new_addr b");
                                    sbRoF.Append(" on a.doro_code = b.doro_code");
                                    sbRoF.Append(" and a.doro_detail = b.doro_detail");
                                    sbRoF.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                    sbRoF.Append(" and sigungu_name = '" + (dr["address_2"].ToString() + " " + dr["address_3"].ToString()) + "'");
                                    sbRoF.Append(" group by doro_name order by CHAR_LENGTH(doro_name) desc, doro_name desc;");
                                    adpt = new MySqlDataAdapter(sbRoF.ToString(), conn);
                                    adpt.Fill(dsRoF, "Tab1");

                                    for (int j = 0; j < dsRoF.Tables[0].Rows.Count; j++)
                                    {
                                        if (dr["address_4"].ToString().Contains(dsRoF.Tables[0].Rows[j]["doro_name"].ToString()))
                                        {
                                            RoF = dsRoF.Tables[0].Rows[j]["doro_name"].ToString();

                                            break;
                                        }
                                    }

                                    if (RoF != "")
                                    {
                                        DataSet dsRoE = new DataSet();
                                        StringBuilder sbRoE = new StringBuilder();
                                        sbRoE.Append(" select");
                                        sbRoE.Append("     detail_name");
                                        sbRoE.Append(" from new_doro a");
                                        sbRoE.Append(" join new_addr b");
                                        sbRoE.Append(" on a.doro_code = b.doro_code");
                                        sbRoE.Append(" and a.doro_detail = b.doro_detail");
                                        sbRoE.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                        sbRoE.Append(" and sigungu_name = '" + (dr["address_2"].ToString() + " " + dr["address_3"].ToString()) + "'");
                                        sbRoE.Append(" and doro_name like '%" + RoF + "%'");
                                        sbRoE.Append(" group by detail_name;");
                                        adpt = new MySqlDataAdapter(sbRoE.ToString(), conn);
                                        adpt.Fill(dsRoE, "Tab1");

                                        if (dsRoE.Tables[0].Rows.Count == 0)
                                        {
                                            DataSet dsRoL = new DataSet();
                                            StringBuilder sbRoL = new StringBuilder();
                                            sbRoL.Append(" select");
                                            sbRoL.Append("     detail_name");
                                            sbRoL.Append(" from new_doro a");
                                            sbRoL.Append(" join new_addr b");
                                            sbRoL.Append(" on a.doro_code = b.doro_code");
                                            sbRoL.Append(" and a.doro_detail = b.doro_detail");
                                            sbRoL.Append(" where a.sido_name = '" + dr["address_1"].ToString() + "'");
                                            sbRoL.Append(" and sigungu_name = '" + (dr["address_2"].ToString() + " " + dr["address_3"].ToString()) + "'");
                                            sbRoL.Append(" and doro_name like '%" + doro_name + "%'");
                                            sbRoL.Append(" group by detail_name;");
                                            adpt = new MySqlDataAdapter(sbRoL.ToString(), conn);
                                            adpt.Fill(dsRoL, "Tab1");

                                            if (dsRoL.Tables[0].Rows.Count == 0)
                                            {
                                            }

                                            sql = "update order_cust_addr set address_level = 0, address_4 = '" + dsRoL.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            sql = "update order_cust_addr set address_level = 0, address_4 = '" + dsRoE.Tables[0].Rows[0]["detail_name"] + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string address = dr["address_3"].ToString();
                                string dong_name = address.Substring(0, address.IndexOf('동') + 1);
                                int chknum = 0;
                                int cnt = 0;
                                string dong_name_new = "";
                                foreach (char c in dong_name)
                                {
                                    if (!int.TryParse(c.ToString(), out chknum))
                                    {
                                        dong_name_new += c;
                                    }

                                    cnt++;
                                }

                                sql = "update order_cust_addr set address_4 = '" + dong_name_new + "', address_level = 0 where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                                MySqlCommand cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            sql = "update order_cust_addr set address_level = 0, address_4 = '" + dongF + "' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        sql = "update order_cust_addr set address_level = 0 ,address_4 = '' where address_1 = '" + dr["address_1"] + "' and address_2 = '" + dr["address_2"] + "' and address_3 = '" + dr["address_3"] + "' and address_4 = '" + dr["address_4"] + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string productCode(string value, DataSet ds, string gubun)
        {
            string[] valueS = value.Split(' ');
            for (int i = 0; i < valueS.Length; i++)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (valueS[i].Contains(ds.Tables[0].Rows[j]["comm_name"].ToString()))
                    {
                        return ds.Tables[0].Rows[j]["comm_code"].ToString();
                    }
                }
            }

            if (gubun == "브랜드")
            {
                return "1036";
            }
            else
            {
                return "2004";
            }
           
        }
    }

    internal class ORDER_LIST_NEW
    {
        public string order_no { get; set; }
        public string base_date { get; set; }
        public string company { get; set; }
        public string brand_code { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string price { get; set; }
        public string cust_no { get; set; }
        public string create_date { get; set; }
    }

    internal class stringModel
    {
        public string sido { get; set; }
        public string sigungu { get; set; }
        public string address_3 { get; set; }
        public string address_4 { get; set; }
        public string mod { get; set; }
        public string no { get; set; }
        public int order { get; set; }
        public string address_level { get; set; }
    }
}