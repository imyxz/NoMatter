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

namespace Client
{
    /// <summary>
    /// AddFolder.xaml 的交互逻辑
    /// </summary>
    public partial class AddFolder : Window
    {
        public AddFolder()
        {
            InitializeComponent();
        }





        private void add_folder_button_Click(object sender, RoutedEventArgs e)
        {
            var name = folder_textbox.Text.Trim();
            if (name == "")
                return;
            ClientSession.AddFolder(name);
            this.Close();
        }
    }
}
