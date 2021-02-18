using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ionpolis.Model
{
    public class dashboardModel
    {
    }

    public class order_list_new
    {
        public string order_no { get; set; }
        public string base_date { get; set; }
        public string company { get; set; }
        public string brand_code { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address { get; set; }
    }

    public class order_list
    {
        public string no { get; set; }
        public string company { get; set; }
        public string order_number { get; set; }
        public string package_number { get; set; }
        public string payment_date { get; set; }
        public string product_name { get; set; }
        public string product_option { get; set; }
        public string amount { get; set; }
        public string customer { get; set; }
        public string tel { get; set; }
        public string address { get; set; }
        public string order_memo { get; set; }
        public string price { get; set; }
        public string state { get; set; }
        public string shipping_fee { get; set; }
        public string state_func { get; set; }
        public string shipping_code { get; set; }
        public string shipping_company { get; set; }
        public string shipping_number { get; set; }
    }
}