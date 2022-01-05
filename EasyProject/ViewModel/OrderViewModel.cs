using EasyProject.Dao;
using EasyProject.Model;
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyProject.ViewModel
{
    public class OrderViewModel : Notifier
    {

        OrderDao dao = new OrderDao();

        public List<DeptModel> Depts { get; set; }

        private DeptModel selectedDept;
        public DeptModel SelectedDept // 콤보박스에서 선택한 부서객체
        {
            get { return selectedDept; }
            set
            {
                selectedDept = value;
                OnPropertyChanged("SelectedDept");
            }
        }

        public OrderViewModel()
        {
            Depts = dao.GetDeptModels();
        }

        private ActionCommand command;
        public ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new ActionCommand(ResetSelectedDept);
                }
                return command;
            }//get

        }//Command

        public void ResetSelectedDept()
        {
            SelectedDept = null;
        }// ProductInsert
    }//OrderViewModel
}//namespace
