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
using JSON;
using HttpRequest;
namespace Client
{
    /// <summary>
    /// RegisteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisteWindow : Window
    {
        public RegisteWindow()
        {
            InitializeComponent();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            HttpQuery query = new HttpQuery();
            //query.RequestCookies.Add(ClientSession.SessionCookie);
            Json json = new Json();
            json["username"] = textbox_username.Text;
            json["password"] = textbox_password.Password;
            json["nickname"] = textbox_nickname.Text;
            json["email"]= textbox_email.Text;
            json["phone"] = textbox_phone.Text;
            try
            {
                var response = Json.Decode(query.Post(ClientConfig.ServerUrl + "user/registe", json.Encode()));
                if (response["status"] == 0)
                {
                    MessageBox.Show("注册成功！");

                    this.Close();
                }
                else
                {
                    MessageBox.Show(response["error"], "注册失败！");

                }
            }
            catch
            {
                MessageBox.Show("注册失败！");
            }
        }
    }
}
