using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CommonClass;
using JSON;
namespace Client
{
    class MainData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string userName="";
        private int curSelectMatterID = 0;
        private MatterBasic matter_info = new MatterOneOff();

        public string UserName { get => userName; set => userName = value; }
        public int CurSelectMatterID { get => curSelectMatterID; set => curSelectMatterID = value; }
        public int User_id { get => matter_info.user_id; set => matter_info.user_id = value; }
        public string Matter_desc { get => matter_info.matter_desc; set => matter_info.matter_desc = value; }
        public Json Matter_addion_info { get => matter_info.matter_addion_info; set => matter_info.matter_addion_info = value; }
        public DateTime Matter_end_time { get => matter_info.matter_end_time; set => matter_info.matter_end_time = value; }
        public MatterType Matter_type { get => matter_info.matter_type; set => matter_info.matter_type = value; }
        public int Matter_id { get => matter_info.matter_id; set => matter_info.matter_id = value; }
        public string Matter_name { get => matter_info.matter_name; set => matter_info.matter_name = value; }
        public DateTime Matter_start_time { get => matter_info.matter_start_time; set => matter_info.matter_start_time = value; }
        public DateTime Matter_next_effect_time { get => matter_info.matter_next_effect_time; set => matter_info.matter_next_effect_time = value; }
    }
}
