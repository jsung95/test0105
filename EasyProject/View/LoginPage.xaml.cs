using Microsoft.Toolkit.Mvvm.DependencyInjection;
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

namespace EasyProject
{
    /// <summary>
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            loginBtn.Click += loginBtn_Click;
            signUpBtn.Click += signUpBtn_Click;
            searchBtn.Click += searchBtn_Click;

            id_TxtBox.Text = Properties.Settings.Default.LoginIDSave;
            if (id_TxtBox.Text.Length > 0)
            {
                id_TxtBox.Focus();
                id_TxtBox.SelectionStart = id_TxtBox.Text.Length;
            }

            Console.WriteLine("Properties : " + Properties.Settings.Default.LoginIDSave);
            Console.WriteLine("isChecked ? : " + Properties.Settings.Default.CheckBoxChecked);
            if (Properties.Settings.Default.CheckBoxChecked == true)
            {
                id_Checkbox.IsChecked = true;
            }
            else
            {
                id_Checkbox.IsChecked = false;
            }

        }


        private void searchBtn_Click(object sender, RoutedEventArgs e) //ID/PW 찾기 버튼 클릭 시
        {
            //throw new NotImplementedException();
            //ID/PW 찾기 페이지 연결
            //MessageBox.Show("PW 변경 버튼 누르셨습니다.");
            NavigationService.Navigate
                (
                new Uri("/View/PasswordChangePage.xaml", UriKind.Relative) // 비밀번호 변경화면
                );
        }
        private void signUpBtn_Click(object sender, RoutedEventArgs e) //회원가입 버튼 클릭 시
        {
            //throw new NotImplementedException();
            //회원가입 창 연결.
            NavigationService.Navigate
                (
                new Uri("/View/SignupPage.xaml", UriKind.Relative) //회원가입화면
                );

        }
        private async void loginBtn_Click(object sender, RoutedEventArgs e) //로그인 버튼 클릭 시
        {
            //await Task.Delay(1500);
            var temp = Ioc.Default.GetService<LoginViewModel>();
            var loginTask = Task.Run(() => temp.Login());
            bool LoginResult = await loginTask; // loginTask가 끝나면 결과를 loginResult에 할당
            Console.WriteLine("LoginResult: " + LoginResult);
            if(LoginResult == true)
            {
                //var button = sender as Button;
                //if (button != null)
                //{
                //    button.Command.Execute(null);
                //}

                if(id_Checkbox.IsChecked == true)
                {
                    Properties.Settings.Default.LoginIDSave = Convert.ToString(App.nurse_dto.Nurse_no);
                    Properties.Settings.Default.CheckBoxChecked = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.LoginIDSave = null;
                    Properties.Settings.Default.CheckBoxChecked = false;
                    Properties.Settings.Default.Save();
                }


                NavigationService.Navigate(new Uri("/View/TabPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("올바른 사번/비밀번호를 입력해주세요.");
            }

/*            var button = sender as Button;
            if (button != null) 
            {
                button.Command.Execute(null);
            }

            NavigationService.Navigate(new Uri("/View/TabPage.xaml", UriKind.Relative));*/
        
        }

        private void checkbox_UnChecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void checkbox_Checked(object sender, RoutedEventArgs e)
        {
        
        }

        private void password_PwBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginBtn_Click(sender, e);
            }
        }
    }
}
