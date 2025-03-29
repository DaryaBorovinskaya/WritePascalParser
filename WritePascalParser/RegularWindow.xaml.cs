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
using System.Windows.Shapes;
using WritePascalParser.ViewModels;

namespace WritePascalParser
{
    /// <summary>
    /// Логика взаимодействия для RegularWindow.xaml
    /// </summary>
    public partial class RegularWindow : Window
    {
        public RegularWindow(int menuNumb)
        {
            InitializeComponent();
            DataContext = new RegularWindowVM(menuNumb);
        }
    }
}
