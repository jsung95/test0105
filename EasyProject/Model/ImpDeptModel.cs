using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class ImpDeptModel : Notifier
    {
        public int? Imp_dept_id { get; set; }
        public int? Imp_dept_count { get; set; }
        public int? Dept_id { get; set; }
        public int? Prod_id { get; set; }


        //table에는 없지만 편의상(그래프 그릴려고) 만듬 
        public string Dept_name { get; set; }
        public string Category_name { get; set; }

    }//class

}//namespace
