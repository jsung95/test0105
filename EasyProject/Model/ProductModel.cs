using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class ProductModel : Notifier
    {
        public int? Prod_id { get; set; }

        private string prod_code;
        public string Prod_code
        {
            get { return prod_code; }
            set
            {
                prod_code = value;
                OnPropertyChanged("Prod_code");
            }
        }

        private string prod_name;
        public string Prod_name
        {
            get { return prod_name; }
            set
            {
                prod_name = value;
                OnPropertyChanged("Prod_name");
            }
        }

        private int? prod_price;
        public int? Prod_price
        {
            get { return prod_price; }
            set
            {
                prod_price = value;
                OnPropertyChanged("Prod_price");
            }
        }

        private int? prod_total;
        public int? Prod_total
        {
            get { return prod_total; }
            set
            {
                prod_total = value;
                OnPropertyChanged("Prod_total");
            }
        }

        private DateTime prod_expire;
        public DateTime Prod_expire
        {
            get { return prod_expire; }
            set
            {
                prod_expire = value;
                OnPropertyChanged("Prod_expire");
            }
        }
        public int? Category_id { get; set; }

    }//class

}//namespace
