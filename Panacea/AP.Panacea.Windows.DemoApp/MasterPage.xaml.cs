using AP.Routing;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace AP.Panacea.Windows.DemoApp
{
    /// <summary>
    /// Interaction logic for MasterPage.xaml
    /// </summary>
    public partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        private void Menu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.Handled)
                return;

            var item = (TreeViewItem)e.NewValue;

            string s = "/" + (string)item.Header;

            object target;
                        
            if (s == "/Home")
            {
                Expression<ResultCreator> targetExpression = () => new HomePage();
                target = targetExpression;
            }
            else if (s == "/About")
                target = new HttpUrl(s);
            else
                target = s;
            
            this.MainContent.Navigate(target);
        }
    }
}
