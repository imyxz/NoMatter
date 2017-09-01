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

        public Main()
        {
            
            InitializeComponent();
            ClientSession.OnMattersChange = MatterFilter;
            ClientSession.OnUserInfoChange = () => { RefreshUserInfo(); return true; };
            ClientSession.OnFoldersChange = FolderFilter;
            ClientSession.ReloadMatters();
            ClientSession.ReloadFolders();
            RefreshUserInfo();
            CloseRight();
        }
        private bool MatterFilter()
        {
            display_matters.Clear();
            string folderName = "所有任务";
            textbox_search.Text = textbox_search.Text.Trim();
            string search = textbox_search.Text;
            foreach(var matter in ClientSession.matters)
            {
                if(FolderFilter(folderName,matter) && SearchFilter(search, matter))
                    display_matters.Add(matter);
            }
            this.MatterList.ItemsSource = display_matters;
            if(search=="")
                this.cur_path.Content = folderName;
            else
                this.cur_path.Content = folderName +" > " +search;
            return true;
        }
        private bool FolderFilter()
        {
            display_folders.Clear();
            MatterFolder tmp;


            tmp = new MatterFolder();
            tmp.folder_name = "所有任务";
            tmp.img_src = "Resources/task_black.png";
            display_folders.Add(tmp);

            tmp = new MatterFolder();
            tmp.folder_name = "今天";
            tmp.img_src = "Resources/today_black.png";
            display_folders.Add(tmp);

            tmp = new MatterFolder();
            tmp.folder_name = "本周";
            tmp.img_src = "Resources/week_black.png";
            display_folders.Add(tmp);

            foreach (var folder in ClientSession.folders)
            {
                folder.img_src = "Resources/list_black.png";
                display_folders.Add(folder);
            }
            this.MatterFolders.ItemsSource = display_folders;
            this.MatterFolders.SelectedIndex = 0;
            return true;
        }
        private bool FolderFilter(string folderName,MatterBasic matter)
        {
            switch(folderName)
            {
                case "所有任务":
                    return true;
                case "今天":
                    return matter.matter_next_effect_time.Date == DateTime.Now.Date;
                case "本周":
                    return ClientHelper.IsSameWeek(matter.matter_next_effect_time, DateTime.Now);
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
                if(result==MessageBoxResult.Yes)
                {
                    ClientSession.SaveMatter(ClientSession.cur_selected_matter);
                }
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
            OnMatterChanged(-1);
        }

        private void textbox_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            MatterFilter();
        }
    }
}
