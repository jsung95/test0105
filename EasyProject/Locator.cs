using EasyProject.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject
{
    public  class Locator
    {
        public ProductViewModel PVM=> Ioc.Default.GetService<ProductViewModel>();
        public LoginViewModel LVM=> Ioc.Default.GetService<LoginViewModel>();
        public SignupViewModel SVM => Ioc.Default.GetService<SignupViewModel>();
        public ProductShowViewModel PSVM => Ioc.Default.GetService<ProductShowViewModel>();
        public PasswordChangeViewModel PCVM => Ioc.Default.GetService<PasswordChangeViewModel>();
        public ProductInOutViewModel PIOVM => Ioc.Default.GetService<ProductInOutViewModel>();
        public DashBoardViewModel DBVM => Ioc.Default.GetService<DashBoardViewModel>();
    }
}
