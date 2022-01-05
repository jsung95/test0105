using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyProject.View
{
    /// <summary>
    /// InsertPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertPage : Page
    {
        public InsertPage()
        {
            InitializeComponent();
            formBtn.Click += formBtn_Click;
            excelBtn.Click += excelBtn_Click;
        }
        private void formBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertPageFrame.Source = new Uri("InsertPage_Form.xaml", UriKind.Relative);
        }
        private void excelBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertPageFrame.Source = new Uri("InsertPage_Excel.xaml", UriKind.Relative);
        }
    }
}
