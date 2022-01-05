using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;

namespace EasyProject.ViewModel
{
    public class ProductInOutViewModel : Notifier
    {
        ProductDao product_dao = new ProductDao();
        DeptDao dept_dao = new DeptDao();

        //입고 내역을 담을 프로퍼티
        private ObservableCollection<ProductInOutModel> product_in;
        public ObservableCollection<ProductInOutModel> Product_in
        {
            get { return product_in; }
            set
            {
                product_in = value;
                OnPropertyChanged("Product_in");
            }
        }
        private ObservableCollection<ProductInOutModel> product_out;
        //출고 내역을 담을 프로퍼티
        public ObservableCollection<ProductInOutModel> Product_out
        {
            get { return product_out; }
            set
            {
                product_out = value;
                OnPropertyChanged("Product_out");
            }
        }

        public ObservableCollection<DeptModel> Depts { get; set; }

        private DeptModel selectedDept_In;
        public DeptModel SelectedDept_In
        {
            get { return selectedDept_In; }
            set
            {
                selectedDept_In = value;
                OnPropertyChanged("SelectedDept_In");
                showInListByDept();
                SelectedStartDate = null;
                SelectedEndDate = null;
            }
        }

        private DeptModel selectedDept_Out;
        public DeptModel SelectedDept_Out
        {
            get { return selectedDept_Out; }
            set
            {
                selectedDept_Out = value;
                OnPropertyChanged("SelectedDept_Out");
                showOutListByDept();
            }
        }

        public string[] SearchTypeList { get; set; }
        public string searchKeyword_In { get; set; }
        public string searchKeyword_Out { get; set; }

        public string selectedSearchType_In { get; set; }
        public string selectedSearchType_Out { get; set; }


        //입출고 날짜별 조회
        //시작일을 담을 프로퍼티
        private DateTime? selectedStartDate;
        public DateTime? SelectedStartDate 
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
                
                ShowProductIn_By_Date();
                if(selectedStartDate > selectedEndDate)
                {
                    SelectedStartDate = SelectedEndDate.Value.AddDays(-1);
                }
                OnPropertyChanged("SelectedStartDate");
            }
        }
        //종료일을 담을 프로퍼티
        private DateTime? selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get { return selectedEndDate; }
            set
            {
                selectedEndDate = value;
                
                ShowProductIn_By_Date();

                OnPropertyChanged("SelectedEndDate");
            }
        }




        public ProductInOutViewModel()
        {
            
            SearchTypeList = new[] { "제품코드", "제품명", "품목/종류" };
            selectedSearchType_In = SearchTypeList[0];

            Depts = new ObservableCollection<DeptModel>(dept_dao.GetDepts());
            SelectedDept_In = Depts[(int)App.nurse_dto.Dept_id - 1];

            selectedSearchType_Out = SearchTypeList[0];
            SelectedDept_Out = Depts[(int)App.nurse_dto.Dept_id - 1];

            Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In));
            Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out));

            SelectedStartDate = null;
            SelectedEndDate = null;

        }//Constructor


        private ActionCommand inSearchCommand;
        public ICommand InSearchCommand
        {
            get
            {
                if (inSearchCommand == null)
                {
                    inSearchCommand = new ActionCommand(InListSearch);
                }
                return inSearchCommand;
            }//get

        }//Command

        public void InListSearch()
        {
            if(SelectedStartDate != null && SelectedEndDate != null)
            {
                Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In, selectedSearchType_In, searchKeyword_In, SelectedStartDate, SelectedEndDate));
            }
            else
            {
                Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In, selectedSearchType_In, searchKeyword_In));
            }

            

        }// InListSearch

        private ActionCommand outSearchCommand;
        public ICommand OutSearchCommand
        {
            get
            {
                if (outSearchCommand == null)
                {
                    outSearchCommand = new ActionCommand(OutListSearch);
                }
                return outSearchCommand;
            }//get

        }//Command

        public void OutListSearch()
        {
            Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out, selectedSearchType_Out, searchKeyword_Out));
       
        }// InListSearch

        public void showInListByDept()
        {
            Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In));
           
        }//showInListByDept

        public void showOutListByDept()
        {
            Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out));
        
        }//showOutListByDept


        public void ShowProductIn_By_Date() // 시작, 끝 날짜 지정해서 입고 데이터 조회
        {
            if(SelectedStartDate != null && SelectedEndDate != null)
            {
                Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In, SelectedStartDate, SelectedEndDate));
            }
        }//ShowProductIn_By_Date

    }//class

}//namespace
