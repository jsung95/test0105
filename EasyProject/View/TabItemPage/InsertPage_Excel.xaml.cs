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
using System.IO;
using Microsoft.Win32;
using EasyProject.ViewModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// InsertPage_Excel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertPage_Excel : Page
    {
        public InsertPage_Excel()
        {
            InitializeComponent();

            fileUploadBtn.Click += fileUploadBtn_Click;
            fileDownLoadBtn.Click += fileDownLoadBtn_Click;
        }

        private void fileDownLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            String result = "제품코드,제품명,품목/종류,유통기한,가격,수량\n";
            result += "ex) A123,주사기,치과재료,2022-11-12,25000,10";
            string today = String.Format(DateTime.Now.ToString("yyyyMMddhhmmss"));
            string f_path = @"c:\temp\재고입력폼" + today + ".csv";
            File.AppendAllText(f_path, result, UnicodeEncoding.UTF8);

            Excel.Application excel_app = new Excel.Application();

            // Make Excel visible (optional).
            excel_app.Visible = true;

            // Open the file.
            excel_app.Workbooks.Open(
                f_path,               // Filename
                Type.Missing,
                Type.Missing,

                   Excel.XlFileFormat.xlCSV,   // Format
                   Type.Missing,
                   Type.Missing,
                   Type.Missing,
                   Type.Missing,

                   ",",          // Delimiter
                   Type.Missing,
                   Type.Missing,
                   Type.Missing,
                   Type.Missing,
                   Type.Missing,
                   Type.Missing
            );
        }

        private void fileUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "csv 파일 (*.csv)|*.csv|엑셀 파일 (*.xls)|*.xls|엑셀 파일 (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {

                //MessageBox.Show(System.IO.Path.GetFullPath(openFileDialog.FileName));
                FileUploadPageFunction uploadPFunction = new FileUploadPageFunction(openFileDialog);
                NavigationService.Navigate(uploadPFunction);
            }
        }
    }
}
