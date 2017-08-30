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
using System.Diagnostics;
using HttpRequest;
using JSON;
namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            HttpQuery query = new HttpQuery();
            Json json = new Json();
            json["username"] = textUserName.Text;
            json["password"] = textPassWord.Text;
            string response=query.Post("http://127.0.0.1:9090/user/checkLogin", json.Encode());
            json = Json.Decode(response);
            Debug.WriteLine(response);
        }
    }
}
