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
using System.Globalization;
using HttpRequest;
using CommonClass;
namespace Client
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Matters display_matters =new Matters();
        public Folders display_folders = new Folders();
        public Folders display_right_folders = new Folders();
        public Main()
        {
            
            InitializeComponent();
            ClientSession.OnMattersChange = MatterRefresh;
            ClientSession.OnUserInfoChange = () => { RefreshUserInfo(); return true; };
            ClientSession.OnFoldersChange = FolderRefresh;
            ClientSession.ReloadFolders();
            ClientSession.ReloadMatters();
            RefreshUserInfo();
            CloseRight();
        }
        private bool MatterRefresh()
        {
            display_matters.Clear();
            var folder = new MatterFolder();
            if(MatterFolders.SelectedItem!=null && MatterFolders.SelectedItem is MatterFolder)
            {
                folder = (MatterFolder)MatterFolders.SelectedItem;
            }
            else
            {
                folder.folder_name = "所有任务";
            }
            textbox_search.Text = textbox_search.Text.Trim();
            string search = textbox_search.Text;
            ClientSession.matters.Sort((MatterBasic a, MatterBasic b) =>
            {
                if (a.is_new) return 1;//新增的排在最后面
                if (b.is_new) return -1;//新增的排在最后面

                return DateTime.Compare(a.matter_next_effect_time, b.matter_next_effect_time);
            });
            foreach (var matter in ClientSession.matters)
            {
                if(FolderFilter(folder, matter) && SearchFilter(search, matter))
                    display_matters.Add(matter);
            }
            
            this.MatterList.ItemsSource = display_matters;
            if(search=="")
                this.cur_path.Content = folder.folder_name;
            else
                this.cur_path.Content = folder.folder_name + " > " +search;
            return true;
        }
        private bool FolderRefresh()
        {
            display_folders.Clear();
            display_right_folders.Clear();
            MatterFolder tmp;
            var count=CountFolderMatter(ClientSession.matters);

            tmp = new MatterFolder();
            tmp.folder_name = "所有任务";
            tmp.Img_src = "task";
            tmp.matter_cnt = count.ContainsKey(-1) ? count[-1] : 0;
            display_folders.Add(tmp);

            tmp = new MatterFolder();
            tmp.folder_name = "今天";
            tmp.Img_src = "day";
            tmp.matter_cnt = count.ContainsKey(-2) ? count[-2] : 0;
            display_folders.Add(tmp);

            tmp = new MatterFolder();
            tmp.folder_name = "本周";
            tmp.Img_src = "week";
            tmp.matter_cnt = count.ContainsKey(-3) ? count[-3] : 0;
            display_folders.Add(tmp);

            tmp = new MatterFolder();
            tmp.folder_name = "邮件";
            tmp.Img_src = "mail";
            tmp.matter_cnt = count.ContainsKey(-4) ? count[-4] : 0;
            display_folders.Add(tmp);

            var tmpfolder = new MatterFolder();
            tmpfolder.folder_id = 0;
            tmpfolder.folder_name = "未分类";
            display_right_folders.Add(tmpfolder);
            foreach (var folder in ClientSession.folders)
            {
                folder.Img_src = "list";
                folder.matter_cnt = count.ContainsKey(folder.folder_id) ? count[folder.folder_id] : 0;
                display_folders.Add(folder);
                display_right_folders.Add(folder);
            }
            this.MatterFolders.ItemsSource = display_folders;
            this.selected_matter_folder_combox.ItemsSource = display_right_folders;
            this.selected_matter_folder_combox.SelectedIndex = 0;
            this.MatterFolders.SelectedIndex = 0;
            return true;
        }
        private Dictionary<int,int> CountFolderMatter(List<MatterBasic> matters)
        {
            var ret = new Dictionary<int, int>();
            
            foreach(var matter in matters)
            {
                if(matter is MatterEmail)
                {
                    ret[-4] = ret.ContainsKey(-4) ? ret[-4] + 1 : 1;//邮件
                }
                else
                {
                    ret[-1] = ret.ContainsKey(-1) ? ret[-1] + 1 : 1;//所有任务
                    if (matter.matter_next_effect_time.Date == DateTime.Now.Date)
                        ret[-2] = ret.ContainsKey(-2) ? ret[-2] + 1 : 1;//今天
                    if (ClientHelper.IsSameWeek(matter.matter_next_effect_time, DateTime.Now))
                        ret[-3] = ret.ContainsKey(-3) ? ret[-3] + 1 : 1;//本周
                }
                ret[matter.folder_id] = ret.ContainsKey(matter.folder_id) ? ret[matter.folder_id] + 1 : 1;

            }
            return ret;
        }
        private bool FolderFilter(MatterFolder folder,MatterBasic matter)
        {
            if(folder.folder_name=="邮件")
            {
                return matter is MatterEmail;
            }
            else if(!(matter is MatterEmail))
            {
                switch (folder.folder_name)
                {
                    case "所有任务":
                        return true;
                    case "今天":
                        return matter.matter_next_effect_time.Date == DateTime.Now.Date;
                    case "本周":
                        return ClientHelper.IsSameWeek(matter.matter_next_effect_time, DateTime.Now);

                }
                return matter.folder_id == folder.folder_id;


            }
            return false;

        }
        private bool SearchFilter(string search, MatterBasic matter)
        {
            var tmp = search.Trim();
            if (tmp == "")
                return true;
            return matter.matter_name.IndexOf(tmp) >= 0 || matter.matter_desc.IndexOf(tmp) >= 0;
        }
        

        public void RefreshUserInfo()
        {
            user_name.Content = ClientSession.user_info.NickName;
        }

        public void CloseRight()
        {
            right_grid.Width = 0;
        }
        public void ShowRight()
        {
            right_grid.Width = 250;
            
        }

        private void button_close_right_Click(object sender, RoutedEventArgs e)
        {
            OnMatterChanged(-1);
        }

        private void MatterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            OnMatterChanged(list.SelectedIndex);
        }
        private void OnMatterChanged(int selection)
        {
            if(ClientSession.is_cur_selected_matter_changed==true && ClientSession.cur_selected_matter!=null)
            {
                var result=MessageBox.Show("是否保存更改信息？", "信息已更改", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    ClientSession.SaveMatter(ClientSession.cur_selected_matter);
                }
                else
                    ClientSession.ReloadMatters();
                ClientSession.is_cur_selected_matter_changed = false;

            }
            if (selection < 0 || selection >= display_matters.Count)
            {
                CloseRight();
                return;
            }
            ClientSession.cur_selected_matter = display_matters[selection];
            UpdateRightGrid();
            ShowRight();
            ClientSession.is_cur_selected_matter_changed = false;

        }
        private void UpdateRightGrid()
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            selected_change_matter_notice_time.Visibility = Visibility.Hidden;
            var tmp = ClientSession.cur_selected_matter;
            selected_matter_name.Text = tmp.matter_name;
            selected_matter_desc.Text = tmp.matter_desc;
            selected_matter_end_time.Text = tmp.matter_end_time.ToString("yyyy-MM-dd") + " 到期";
            selected_matter_notice_time.Text="在 "+ tmp.matter_next_effect_time.ToString("H:mm") +" 时提醒我";
            selected_matter_notice_date.Text = "在 " + tmp.matter_next_effect_time.ToString("yyyy年M月d日");
            selected_matter_create_time.Content = tmp.matter_start_time.ToString("创建于: yyyy-MM-dd");
            selected_matter_folder_combox.SelectedIndex = ClientSession.FindFolderIndex(tmp.folder_id, ClientSession.folders);
            right_check_box.IsChecked = false;
            if (tmp is MatterPeriodicity tmp1)
            {
                selected_matter_cycle_combox.SelectedIndex = (int)tmp1.period_type;
            }
            else
            {
                selected_matter_cycle_combox.SelectedIndex = 0;
            }
        }



        private void selected_matter_end_time_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = ClientSession.cur_selected_matter;
            selected_matter_end_time.Text = tmp.matter_end_time.ToString("yyyy-MM-dd H:mm");

        }

        private void selected_matter_end_time_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = ClientSession.cur_selected_matter;
            var time = new DateTime();
            if (!DateTime.TryParse(selected_matter_end_time.Text, out time))
            {
                MessageBox.Show("错误的时间日期！请按 年-月-日 时:分 的形式输入", "解析错误");
            }
            else
            {
                if (DateTime.Compare(time, ClientSession.cur_selected_matter.matter_end_time) != 0)
                    ClientSession.is_cur_selected_matter_changed = true;
                ClientSession.cur_selected_matter.matter_end_time = time;
            }
            UpdateRightGrid();

        }

        private void selected_matter_notice_time_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = ClientSession.cur_selected_matter;
            selected_change_matter_notice_time.Visibility = Visibility.Visible;
            selected_change_matter_notice_time.Text = tmp.matter_next_effect_time.ToString("yyyy-MM-dd H:mm");

        }

        private void selected_change_matter_notice_time_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = ClientSession.cur_selected_matter;
            var time = new DateTime();
            if (!DateTime.TryParse(selected_change_matter_notice_time.Text, out time))
            {
                MessageBox.Show("错误的时间日期！请按 年-月-日 时:分 的形式输入", "解析错误");
            }
            else
            {
                if(DateTime.Compare(time, ClientSession.cur_selected_matter.matter_next_effect_time) !=0)
                    ClientSession.is_cur_selected_matter_changed = true;
                ClientSession.cur_selected_matter.matter_next_effect_time = time;
            }
            UpdateRightGrid();
        }

        private void selected_matter_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = selected_matter_name.Text;
            if (tmp!= ClientSession.cur_selected_matter.matter_name)
            {
                ClientSession.cur_selected_matter.matter_name = tmp;
                ClientSession.is_cur_selected_matter_changed = true;
            }
            
        }

        private void selected_matter_desc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var tmp = selected_matter_desc.Text;
            if (tmp != ClientSession.cur_selected_matter.matter_desc)
            {
                ClientSession.cur_selected_matter.matter_desc = tmp;
                ClientSession.is_cur_selected_matter_changed = true;
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selected_matter_name_LostFocus(null, null);
            selected_matter_desc_LostFocus(null, null);

            MatterList.SelectedIndex = -1;
        }

        private void textbox_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            MatterRefresh();
        }

        private void MatterFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MatterRefresh();
        }


        private void new_task_textblock_LostFocus(object sender, RoutedEventArgs e)
        {
            new_task_textblock.Visibility = Visibility.Hidden;
        }

        private void new_task_label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new_task_textblock.Visibility = Visibility.Visible;
            new_task_textblock.Text = "";
            new_task_textblock.Focus();
        }

        private void new_task_textblock_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter && new_task_textblock.Text!="")
            {
                var matter = new MatterOneOff();
                matter.matter_name = new_task_textblock.Text;
                matter.is_new = true;
                ClientSession.matters.Add(matter);
                MatterFolders.SelectedIndex = 0;
                textbox_search.Text = "";
                MatterRefresh();
                MatterList.SelectedIndex = MatterList.Items.Count-1;
                ClientSession.is_cur_selected_matter_changed = true;
                
            }
        }

        private void selected_matter_cycle_combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            ClientSession.cur_selected_matter.matter_addion_info["period_type"] = ((MatterPeriodicityType)selected_matter_cycle_combox.SelectedIndex).ToString();
            ClientSession.is_cur_selected_matter_changed = true;
        }

        private void button_delete_right_Click(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            if(ClientSession.cur_selected_matter.is_new)
            {
                ClientSession.ReloadMatters();
                return;
            }
            ClientSession.is_cur_selected_matter_changed = false;
            ClientSession.DeleteMatter(ClientSession.cur_selected_matter.matter_id);
        }

        private void renew_matter_button_Click(object sender, RoutedEventArgs e)
        {
            ClientSession.ReloadFolders();
            ClientSession.ReloadMatters();
        }

        private void Finish_Checked(object sender, RoutedEventArgs e)
        {
            var matter_id = (int)((CheckBox)sender).Tag;
            if (matter_id <= 0)
                return;
            ClientSession.FinishMatter(matter_id);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            var matter_id = ClientSession.cur_selected_matter.matter_id;
            if (matter_id <= 0)
                return;
            ClientSession.FinishMatter(matter_id);
        }

        private void button_add_folder_Click(object sender, RoutedEventArgs e)
        {
            (new AddFolder()).Show();

        }

        private void selected_matter_folder_combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientSession.cur_selected_matter == null)
                return;
            if(((ComboBox)sender).SelectedItem is MatterFolder folder)
            {
                ClientSession.cur_selected_matter.folder_id = folder.folder_id;
                ClientSession.is_cur_selected_matter_changed = true;

            }
        }

        private void button_delete_folder_Click(object sender, RoutedEventArgs e)
        {
            var folder_id = (int)((Button)sender).Tag;
            ClientSession.DeleteFolder(folder_id);
        }

        private void button_edit_mail_Click(object sender, RoutedEventArgs e)
        {
            (new MailboxManage()).Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MatterList.MaxHeight = MainWindow.Height - 200;
        }
    }
}
