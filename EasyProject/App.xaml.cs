using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasyProject.Model;
using EasyProject.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace EasyProject
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static NurseModel nurse_dto = null;
        public static CategoryModel category_dto = null;
        public App()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<ProductViewModel>()
                .AddSingleton<LoginViewModel>()
                .AddSingleton<ProductShowViewModel>()
                .AddSingleton<SignupViewModel>()
                .AddSingleton<PasswordChangeViewModel>()
                .AddSingleton<ProductInOutViewModel>()
                .AddSingleton<DashBoardViewModel>()
                .BuildServiceProvider());

            this.InitializeComponent();
            nurse_dto = new NurseModel();
            Console.WriteLine("===> Init NurseDTO");
            Console.WriteLine("  Nurse NO : {0}", nurse_dto.Nurse_no);
            Console.WriteLine("  Nurse NAME : {0}", nurse_dto.Nurse_name);
            Console.WriteLine("  Nurse AUTH : {0}", nurse_dto.Nurse_auth);
            Console.WriteLine("  DEPT ID : {0}", nurse_dto.Dept_id);
        }//App Constructor
    }
}
