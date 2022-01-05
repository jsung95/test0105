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
using EasyProject.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Text.RegularExpressions;

namespace EasyProject.View
{
    /// <summary>
    /// ReleasePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReleasePage : Page
    {
        public ReleasePage()
        {
            InitializeComponent();

            dept_Label.Visibility = Visibility.Hidden;
            Dept_comboBox.Visibility = Visibility.Hidden;
        }




        private void reset_Btn_Click(object sender, RoutedEventArgs e)
        {

            mount_TxtBox_Hidden.Text = "";
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
               (
               new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative)
               );
        }

        private async void release_Btn_Click(object sender, RoutedEventArgs e)
        {
            var temp = Ioc.Default.GetService<ProductShowViewModel>();

            if(Type_comboBox.SelectedValue != null)
            {
                if (Type_comboBox.SelectedValue.Equals("사용"))
                {
                    if(mount_TxtBox.Text.Length == 0) //사용일때 수량입력을 안했다면
                    {
                        mount_TxtBox.Focus();
                        temp.ErrorProductString = "제품 사용 수량을 입력해주세요.";
                        temp.IsEmptyProduct = true;
                    }//if
                    else
                    {
                        var releaseTask = Task.Run(() => temp.OutProduct());
                        await releaseTask;
                        NavigationService.Navigate(new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative));
                    }//else

                }//if
                else if (Type_comboBox.SelectedValue.Equals("이관"))
                {
                    if(Dept_comboBox.SelectedValue == null) //이관일때 부서를 안고르면
                    {
                        Dept_comboBox.Focus();
                        temp.ErrorProductString = "제품을 이관할 부서를 선택해주세요.";
                        temp.IsEmptyProduct = true;
                    }//if
                    else
                    {
                        if (mount_TxtBox.Text.Length == 0) //부서를 골랐는데 수량입력을 안했다면
                        {
                            mount_TxtBox.Focus();
                            temp.ErrorProductString = "제품 사용 수량을 입력해주세요.";
                            temp.IsEmptyProduct = true;
                        }//if
                        else
                        {
                            var releaseTask = Task.Run(() => temp.OutProduct());
                            await releaseTask;
                            NavigationService.Navigate(new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative));
                        }//else

                    }//else

                }//else-if
                else
                {
                    var releaseTask = Task.Run(() => temp.OutProduct());
                    await releaseTask;
                    NavigationService.Navigate(new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative));
                }//else

            }//if
            else
            {
                Type_comboBox.Focus();
                temp.ErrorProductString = "출고 유형을 선택해주세요.";
                temp.IsEmptyProduct = true;
            }//else

            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = Ioc.Default.GetService<ProductShowViewModel>();

            if (Type_comboBox.SelectedValue != null)
            {
                if (Type_comboBox.SelectedValue.Equals("사용"))
                {
                    dept_Label.Visibility = Visibility.Hidden;
                    Dept_comboBox.Visibility = Visibility.Hidden;

                    mount_TxtBox.Text = null;

                    mount_TxtBox_Hidden.IsEnabled = true;
                    mount_TxtBox_Hidden.Visibility = Visibility.Hidden;
                }
                else if (Type_comboBox.SelectedValue.Equals("폐기"))
                {
                    dept_Label.Visibility = Visibility.Hidden;
                    Dept_comboBox.Visibility = Visibility.Hidden;

                    mount_TxtBox.Text = Convert.ToString(temp.SelectedProduct.Imp_dept_count);
                    mount_TxtBox.Focus();

                    mount_TxtBox_Hidden.Visibility = Visibility.Visible;
                    mount_TxtBox_Hidden.Text = Convert.ToString(temp.SelectedProduct.Imp_dept_count);
                    mount_TxtBox_Hidden.IsEnabled = false;
                }
                else //이관
                {
                    dept_Label.Visibility = Visibility.Visible;
                    Dept_comboBox.Visibility = Visibility.Visible;

                    mount_TxtBox.Text = null;

                    mount_TxtBox_Hidden.IsEnabled = true;
                    mount_TxtBox_Hidden.Visibility = Visibility.Hidden;
                }
            }

            /*ComboBox currentComboBox = sender as ComboBox;
            
            if (currentComboBox != null)
            {

                ComboBoxItem currentItem = currentComboBox.SelectedItem as ComboBoxItem;
                Console.WriteLine(currentItem);
                if (currentItem.Content.Equals("사용")|| currentItem.Content.Equals("폐기"))
                {
                    
                    Dept_comboBox.Visibility = Visibility.Hidden;
                }

                else
                {
                    Dept_comboBox.Visibility = Visibility.Visible;

                }

            }*/


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
