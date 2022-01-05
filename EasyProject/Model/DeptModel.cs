using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class DeptModel : Notifier
    {

        public int? Dept_id { get; set; }

        private string dept_name;
        public string Dept_name
        {
            get { return dept_name; }
            set
            {
                dept_name = value;
                //OnPropertyChanged("Dept_name");
            }
        } // Dept_name

        public string Dept_phone { get; set; }

        public string Dept_status { get; set; }

    } // DeptModel
} // namespace
