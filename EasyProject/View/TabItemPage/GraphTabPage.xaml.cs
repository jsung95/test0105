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

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// GraphTabPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GraphTabPage : Page
    {
        public GraphTabPage()
        {
            InitializeComponent();
        }

        private void TabButtonClick(object sender, RoutedEventArgs e)       //버튼 창닫기
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness((150 * index), 0, 0, 0);

            switch (index)
            {
                case 0:
                    TabFrame.Source = new Uri("GraphPage/AllGraphPage.xaml", UriKind.Relative);
                    break;
                case 1:
                    TabFrame.Source = new Uri("GraphPage/ChoiceGraphPage.xaml", UriKind.Relative);
                    break;
               
            }
        }
    }
}
