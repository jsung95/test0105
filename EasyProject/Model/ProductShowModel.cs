using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyProject.Model
{
    public class ProductShowModel : Notifier
    {

        public string Prod_code { get; set; }
        public string Prod_name { get; set; }
        public string Category_name { get; set; }
        public int? Prod_price { get; set; }
        public int? Prod_total { get; set; }
        public int? Imp_dept_count { get; set; }
        public DateTime Prod_expire { get; set; }
        public int? Prod_id { get; set; }
        public int? Imp_dept_id { get; set; }
        public int? Prod_remainexpire { get; set; }
        // 재고 출고 - 출고 유형 콤보박스에 들어갈 리스트
        private string[] selectedOutTypeList = new[] { "사용", "이관", "폐기" };
        public string[] SelectedOutTypeList
        {
            get { return selectedOutTypeList; }
            set
            {
                selectedOutTypeList = value;
                OnPropertyChanged("SelectedOutTypeList");
            }
        }

        // 재고 출고 - 선택한 출고 유형 콤보박스를 담을 값
        private string selectedOutType;
        public string SelectedOutType
        {
            get { return selectedOutType; }
            set
            {
                selectedOutType = value;
                IsEnabled = true;
                Console.WriteLine("SelectedOutType 변경합니다! : " + selectedOutType);
                if (selectedOutType == "사용")
                {
                    Popup_combobox_vis = Visibility.Hidden;
                    Popup_textBox_vis = Visibility.Hidden;
                }
                else if (selectedOutType == "이관")
                {
                    Popup_combobox_vis = Visibility.Visible;
                    Popup_textBox_vis = Visibility.Visible;
                }
                else if (selectedOutType == "폐기")
                {
                    Popup_combobox_vis = Visibility.Hidden;
                    Popup_textBox_vis = Visibility.Hidden;

                }

                OnPropertyChanged("SelectedOutType");
            }
        }
        // 출고 팝업박스 - 부서 선택 콤보 박스의 Visibility와 바인딩
        private Visibility popup_combobox_vis;
        public Visibility Popup_combobox_vis
        {
            get { return popup_combobox_vis; }
            set
            {
                popup_combobox_vis = value;
                Console.WriteLine("popup_combobox_vis 변경합니다! : " + popup_combobox_vis);
                OnPropertyChanged("Popup_combobox_vis");
            }
        }
        // 출고 - 부서 선택 콤보 박스 좌측 textbox의 Visibility와 바인딩
        private Visibility popup_textBox_vis;
        public Visibility Popup_textBox_vis
        {
            get { return popup_textBox_vis; }
            set
            {
                popup_textBox_vis = value;
                Console.WriteLine("popup_textBox_vis 변경합니다! : " + popup_textBox_vis);
                OnPropertyChanged("Popup_textBox_vis");
            }
        }

        // 재고 출고 - 선택한 출고(이관) 부서를 담을 프로퍼티
        private DeptModel selectedOutDept;
        public DeptModel SelectedOutDept
        {
            get { return selectedOutDept; }
            set
            {
                selectedOutDept = value;
                //Console.WriteLine("선택한 출고(이관) 부서명 : " + selectedOutDept.Dept_name);
                OnPropertyChanged("SelectedOutDept");
            }
        }

        // 재고 출고 - 입력한 출고 수량을 담을 프로퍼티
        private int? inputOutCount;
        public int? InputOutCount
        {
            get { return inputOutCount; }
            set
            {
                inputOutCount = value;
                OnPropertyChanged("InputOutCount");
            }
        }

        //재고 출고 - 확인 버튼 활성화/비활성화 프로퍼티
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
    }//class

}//namespace
