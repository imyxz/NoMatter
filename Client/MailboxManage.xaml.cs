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
using System.Collections.ObjectModel;
using CommonClass;
namespace Client
{
    /// <summary>
    /// MailboxManage.xaml 的交互逻辑
    /// </summary>
    public partial class MailboxManage : Window
    {
        public int cur_select_mailbox_id = 0;
        public Mailboxs display_mailboxs = new Mailboxs();
        public MailboxManage()
        {
            InitializeComponent();
            ClientSession.OnMailBoxChange = RefreshMailbox;
            ClientSession.ReloadMailboxs();
        }
        public bool RefreshMailbox()
        {
            display_mailboxs.Clear();
            foreach (var mailbox in ClientSession.mailboxes)
                display_mailboxs.Add(mailbox);
            mailbox_list.ItemsSource = display_mailboxs;
            return true;

        }

        private void mailbox_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mailbox = (Mailbox)((ListBox)sender).SelectedItem;
            if(mailbox!=null)
            {
                textbox_email.Text = mailbox.Email_address;
                textbox_host.Text = mailbox.pop3_address;
                textbox_password.Text = mailbox.email_password;
                textbox_port.Text = mailbox.pop3_port.ToString();
                checkbox_usessl.IsChecked = mailbox.use_ssl;
                cur_select_mailbox_id = mailbox.mailbox_id;
            }
        }

        private void new_button_Click(object sender, RoutedEventArgs e)
        {
            var mailbox = new Mailbox();
            mailbox.Email_address = textbox_email.Text;
            mailbox.pop3_address = textbox_host.Text;
            mailbox.email_password = textbox_password.Text;
            int a = 0;
            Int32.TryParse(textbox_port.Text, out a);
            mailbox.pop3_port = a;
            mailbox.use_ssl = checkbox_usessl.IsChecked??false;
            mailbox.mailbox_id = 0;
            ClientSession.SaveMailBox(mailbox);
            MessageBox.Show("修改成功！");
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            var mailbox = new Mailbox();
            mailbox.Email_address = textbox_email.Text;
            mailbox.pop3_address = textbox_host.Text;
            mailbox.email_password = textbox_password.Text;
            int a = 0;
            Int32.TryParse(textbox_port.Text, out a);
            mailbox.pop3_port = a;
            mailbox.use_ssl = checkbox_usessl.IsChecked ?? false;
            mailbox.mailbox_id = cur_select_mailbox_id;
            ClientSession.SaveMailBox(mailbox);
            MessageBox.Show("修改成功！");

        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            ClientSession.DeleteMailBox(cur_select_mailbox_id);
            MessageBox.Show("修改成功！");

        }
    }
    public class Mailboxs : ObservableCollection<Mailbox>
    {
        public Mailboxs():base() { }
    }
}
