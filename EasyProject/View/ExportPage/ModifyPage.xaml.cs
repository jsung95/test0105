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
using System.Text.RegularExpressions;
using EasyProject.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace EasyProject.View
{
    /// <summary>
    /// ModifyPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModifyPage : Page
    {
        public ModifyPage()
        {
            InitializeComponent();

        }

        private void reset_Btn_Click(object sender, RoutedEventArgs e)
        {
            prodcode_TxtBox.Text = "";
            prodname_TxtBox.Text = "";
            price_TxtBox.Text = "";
            mount_TxtBox.Text = "";
            expirationDate_DatePicker.SelectedDate = null;
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
                (
                new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative)
                );
        }

        private async void modify_Btn_Click(object sender, RoutedEventArgs e)
        {
            var temp = Ioc.Default.GetService<ProductShowViewModel>();

            //만약 입력을 하나라도 안할경우 메시지 출력
            if(prodcode_TxtBox.Text.Length < 1 || prodname_TxtBox.Text.Length < 1 || expirationDate_DatePicker.Text.Length < 1 || price_TxtBox.Text.Length < 1 || mount_TxtBox.Text.Length < 1)
            {
                temp.ErrorProductString = "수정할 제품의 정보를 모두 기입해주세요.";
                temp.IsEmptyProduct = true;
            }
            else
            {
                var modifyTask = Task.Run(() => temp.ChangeProductInfo());
                await modifyTask; // ChangeProductInfo() 메소드가 끝날때까지 기다렸다가 페이지 전환
                NavigationService.Navigate(new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative));
            }
        }


        // 가격 텍스트박스의 입력값이 변경되었을 때 값이 반영되기전에 들어오는 이벤트 
        // 따라서 정규식 검사 클래스인 Regex를 이용하여 숫자일 때에만 수정이 가능하게 구현.
        private void price_TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // 수량 텍스트박스의 입력값이 변경되었을 때 값이 반영되기전에 들어오는 이벤트 
        // 따라서 정규식 검사 클래스인 Regex를 이용하여 숫자일 때에만 수정이 가능하게 구현.
        private void mount_TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
