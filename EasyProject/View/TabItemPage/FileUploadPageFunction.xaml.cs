using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using System.Windows.Data;
using EasyProject.ViewModel;
using EasyProject.Model;
using System.Collections.Generic;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// FileUploadPageFunction.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileUploadPageFunction : PageFunction<String>
    {
        private OpenFileDialog openFileDialog;

        public FileUploadPageFunction(OpenFileDialog openFileDialog)
        {
            InitializeComponent();
            //this.DataContext = new ProductViewModel();

            fileUploadBtn.Click += fileUploadBtn_Click;

            this.openFileDialog = openFileDialog;

            ((ProductViewModel)(this.DataContext)).OpenFileDialog = openFileDialog.FileName;
            SetFileNameTxtBlock();
        }

        private string GetFileName(OpenFileDialog openFileDialog)
        {
            return System.IO.Path.GetFileName(openFileDialog.FileName);
        }
        private void SetFileNameTxtBlock()
        {
            fileNameTxtbox.Text = GetFileName(this.openFileDialog);
        }

        private void fileUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            //이전 페이지로 돌아가기 (PageFunction 객체 생성한 페이지)
            OnReturn(new ReturnEventArgs<string>());
        }
    }
}
