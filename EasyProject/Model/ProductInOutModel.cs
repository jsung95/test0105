using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class ProductInOutModel : Notifier
    {
        //PRODUCT
        public string Prod_code { get; set; }
        public string Prod_name { get; set; }
        public DateTime Prod_expire { get; set; }
        public int? Prod_price { get; set; }

        //CATEGORY
        public string Category_name { get; set; }

        //PRODUCT_IN
        public int? Prod_in_count { get; set; }
        public DateTime Prod_in_date { get; set; }
        public string Prod_in_from { get; set; }
        public string Prod_in_to { get; set; }
        public string Prod_in_type { get; set; }
        public int? prod_order_cases { get; set; } // 발주 횟수
        public int? prod_transferIn_cases { get; set; } // 이관 횟수

        //PRODUCT_OUT
        public int? Prod_out_count { get; set; }
        public DateTime Prod_out_date { get; set; }
        public string Prod_out_content { get; set; }
        public string Prod_out_from { get; set; }
        public string Prod_out_to { get; set; }
        public string Prod_out_type { get; set; }
        public int? prod_use_cases { get; set; } // 사용 횟수
        public int? prod_transferOut_cases { get; set; } // 이관 횟수
        public int? prod_discard_cases { get; set; } // 폐기 횟수

        //NURSE
        public string Nurse_name { get; set; }

        //DEPT
        public string Dept_name { get; set; }

    }//class

}//namespace
