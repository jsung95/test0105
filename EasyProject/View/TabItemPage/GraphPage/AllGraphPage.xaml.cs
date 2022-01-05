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

namespace EasyProject.View.TabItemPage.GraphPage

{
    /// <summary>
    /// GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AllGraphPage : Page
    {
        public String userDept = null;
        public AllGraphPage()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            //deptName_ComboBox1.SelectedIndex = (int)App.nurse_dto.Dept_id - 1;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var temp = Ioc.Default.GetService<DashBoardViewModel>();
            //temp.DashboardPrint();
            temp.DashboardPrint1(temp.SelectedDept1, temp.SelectedCategory1);
            temp.DashboardPrint2();
            temp.DashboardPrint3();
            temp.DashboardPrint4(temp.SelectedCategory1);
            temp.DashboardPrint_Pie();

        }
    }
}
