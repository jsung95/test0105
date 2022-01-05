using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Collections;
using EasyProject.Model;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using EasyProject.ViewModel;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// StatusPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatusPage : Page
    {
        public ChartValues<float> Values { get; set; }

        public bool isComboBoxDropDownOpened = false;

        public StatusPage()
        {
            InitializeComponent();
            //dept_Label.Visibility = Visibility.Hidden;
            //Dept_comboBox.Visibility = Visibility.Hidden;

            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
            var deptNameText = deptModelObject.Dept_name; // 콤보박스에서 선택한 부서명
            var temp = Ioc.Default.GetService<ProductShowViewModel>();
            var userDept = temp.Depts[(int)App.nurse_dto.Dept_id - 1];  // 현재 사용자 소속 부서 객체
            var userDeptName = userDept.Dept_name;

            var dash = Ioc.Default.GetService<ProductShowViewModel>();
            //temp.DashboardPrint();
            dash.DashboardPrint1(dash.SelectedDept, dash.SelectedCategory1);
            dash.DashboardPrint2(dash.SelectedDept);

            if (deptNameText.Equals(userDeptName) || userDeptName == null)
            {
                Console.WriteLine(userDeptName + "같은 부서일때");
                buttonColumn.Visibility = Visibility.Visible;
            }
            else
            {
                Console.WriteLine(userDeptName + "다른 부서일때");
                buttonColumn.Visibility = Visibility.Hidden;
            }

            ((ProductShowViewModel)(this.DataContext)).LoadEmployee();

        }
        private void reset_Btn_Click(object sender, RoutedEventArgs e)
        {
            //mount_TxtBox.Text = "";
        }

        private void RowButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("버튼을 클릭했습니다.");
        }


        private void Part_comboBox_Selection(object sender, SelectedCellsChangedEventArgs e)
        {
            MessageBox.Show("버튼을 클릭했습니다.");
        }

        /*private void goDialog_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
                (
                new Uri("/View/ExportPage/DialogPage.xaml", UriKind.Relative)
                );
        }*/

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            isComboBoxDropDownOpened = true;

            var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
            var deptNameText = deptModelObject.Dept_name; // 콤보박스에서 선택한 부서명
            var temp = Ioc.Default.GetService<ProductShowViewModel>();
            var userDept = temp.Depts[(int)App.nurse_dto.Dept_id - 1];  // 현재 사용자 소속 부서 객체
            var userDeptName = userDept.Dept_name;
            if (isComboBoxDropDownOpened)
            {

                if (deptNameText.Equals(userDeptName) || userDeptName == null)
                {
                    Console.WriteLine(userDeptName + "같은 부서일때");
                    buttonColumn.Visibility = Visibility.Visible;
                }
                else
                {
                    Console.WriteLine(userDeptName + "다른 부서일때");
                    buttonColumn.Visibility = Visibility.Hidden;
                }
            }
        }

        private void DataGridCheckboxClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCheckbox.IsChecked == true)
            {
                //DataAndGraphGrid.ColumnDefinitions.Add(DataGridColumn);
                DataGridColumn.Width = new GridLength(1.8, GridUnitType.Star);
            }
            else
            {
                DataGridColumn.Width = new GridLength(0);
            }
        }

        private void GraphCheckboxClick(object sender, RoutedEventArgs e)
        {
            if (GraphCheckbox.IsChecked == true)
            {
                GraphColumn.Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                GraphColumn.Width = new GridLength(0);
            }
        }

        private void GraphCheckboxUnChecked(object sender, RoutedEventArgs e)
        {
            GraphCard.Visibility = Visibility.Visible;
        }
        private void reset_Btn_Click(object sender, RoutedEvent e)
        {
            //mount_TxtBox.Text = "";
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void signUp_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = Ioc.Default.GetService<ProductShowViewModel>();

            //if (Type_comboBox.SelectedValue != null)
            //{
            //    if (Type_comboBox.SelectedValue.Equals("사용"))
            //    {
            //        Dept_comboBox.Visibility = Visibility.Hidden;

            //        mount_TxtBox.Text = null;

            //        mount_TxtBox_Hidden.IsEnabled = true;
            //        mount_TxtBox_Hidden.Visibility = Visibility.Hidden;
            //    }
            //    else if (Type_comboBox.SelectedValue.Equals("폐기"))
            //    {
            //        Dept_comboBox.Visibility = Visibility.Hidden;

            //        mount_TxtBox.Text = Convert.ToString(temp.SelectedProduct.Imp_dept_count);
            //        mount_TxtBox.Focus();

            //        mount_TxtBox_Hidden.Visibility = Visibility.Visible;
            //        mount_TxtBox_Hidden.Text = Convert.ToString(temp.SelectedProduct.Imp_dept_count);
            //        mount_TxtBox_Hidden.IsEnabled = false;

            //        Console.WriteLine("ori : " + mount_TxtBox.Text);
            //        Console.WriteLine("ori enable? : " + mount_TxtBox.IsEnabled);
            //        Console.WriteLine("aft : " + mount_TxtBox_Hidden.Text);
            //        Console.WriteLine("aft enable? : " + mount_TxtBox_Hidden.IsEnabled);
            //    }
            //    else
            //    {
            //        Dept_comboBox.Visibility = Visibility.Visible;

            //        mount_TxtBox.Text = null;

            //        mount_TxtBox_Hidden.IsEnabled = true;
            //        mount_TxtBox_Hidden.Visibility = Visibility.Hidden;
            //    }
            //}
        }


    }

}
