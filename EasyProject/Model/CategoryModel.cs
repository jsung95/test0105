using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class CategoryModel
    {
        public int? Category_id { get; set; }
        private string category_name;
        public string Category_name
        {
            get { return category_name; }
            set
            {
                category_name = value;
                //OnPropertyChanged("category_name");
            }

        }

    }//class

}//namespace
