using EEMC.Models;
using EEMC.ViewBases;
using EEMC.ViewModels;
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

namespace EEMC.Views
{
    /// <summary>
    /// Логика взаимодействия для CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Page, IInitWebView2
    {
        public CourseWindow()
        {
            InitializeComponent();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;

            var item = CourseTreeView.ItemContainerGenerator.ContainerFromItem(CourseTreeView.SelectedItem) as TreeViewItem;

            if (item != null)
                item.IsSelected = false;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await (this as IInitWebView2).InitializeWebView2(webView);

            //Устанавливаем Source у WebView2
            webView.Source = (this.DataContext as CourseWindowVM).PdfPath;
        }

        private void CourseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = CourseTreeView.SelectedItem as Explorer;

            //Устанавливаем Source у WebView2
            var dc = this.DataContext as CourseWindowVM;

            dc.ShowFile_Click.Execute(item);

            webView.Source = dc.PdfPath;
        }
    }
}
