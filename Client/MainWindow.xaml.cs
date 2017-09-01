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
using CommonClass;
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
            button_Click(null, null);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            HttpQuery query = new HttpQuery();
            //query.RequestCookies.Add(ClientSession.SessionCookie);
            Json json = new Json();
            json["username"] = textUserName.Text;
            json["password"] = textPassWord.Text;
            try
            {
                var response =Json.Decode(query.Post(ClientConfig.ServerUrl+ "user/checkLogin", json.Encode()));
                if(response["status"]==0)
                {
                    ClientSession.user_info = response["info"]["user_info"].ConvertTo<UserInfo>();
                    ClientSession.SessionCookie = query.ResponseCookies[0];
                    
                    var main = new Main();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response["error"], "登录失败！");

                }
            }
            catch
            {
                MessageBox.Show( "登录失败！");
            }
            
        }
    }
}
