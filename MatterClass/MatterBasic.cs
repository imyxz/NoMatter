using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace CommonClass
{
    public abstract class MatterBasic:IJsonAble
    {
        public int user_id=0;
        public string matter_desc="";
        public Json matter_addion_info=new Json();
        public DateTime matter_end_time= DateTime.Now;
        public MatterType matter_type=MatterType.oneOffEvent;
        public int matter_id=0;
        public string matter_name="";
        public DateTime matter_start_time = DateTime.Now;
        public DateTime matter_next_effect_time = DateTime.Now;
        public int folder_id=0;
        public bool is_new = false;//创建时标记一下
        public int is_noticed = 0;
        public string Test
        {
            get { return "1"; }
            set { }
        }

        public int Matter_id { get => matter_id; set => matter_id = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public string Matter_name { get => matter_name; set => matter_name = value; }
        public string Matter_desc { get => matter_desc; set => matter_desc = value; }
        public Json Matter_addion_info { get => matter_addion_info; set => matter_addion_info = value; }
        public DateTime Matter_start_time { get => matter_start_time; set => matter_start_time = value; }
        public string Matter_next_effect_time_style { get {
                if (matter_next_effect_time < DateTime.Now)
                {
                    return "Red";
                }
                else
                    return "DeepSkyBlue";
            } }
        public string Matter_next_effect_time { get {
                if (matter_next_effect_time.Date == DateTime.Now.Date)
                    return "今天 " + matter_next_effect_time.ToString("HH:mm");
                else
                    return matter_next_effect_time.ToString("yyyy-MM-dd");

            }
        }
        public DateTime Matter_end_time { get => matter_end_time; set => matter_end_time = value; }
        public MatterType Matter_type { get => matter_type; set => matter_type = value; }

        public int GetID()
        {
            return Matter_id;
        }

        public MatterType GetMatterType()
        {
            return Matter_type;
        }

        public string GetName()
        {
            return Matter_name;
        }

        public string GetDesc()
        {
            return Matter_desc;
        }

        public DateTime GetStartTime()
        {
            return Matter_start_time;
        }

        public abstract DateTime GetNextEffectTime();

        public DateTime GetEndTime()
        {
            return Matter_end_time;
        }

        public int GetUserID()
        {
            return User_id;
        }

        public abstract Object fromJson(Json json);

        public virtual Json toJson()
        {
            return Json.ConvertFrom(this, (string c, string m) =>
            {
                switch (m)
                {
                    case "is_new":
                        return false;
                }
                return true;
            });
        }

        public static MatterBasic ParseFromDB(IDictionary<string, string> info)
        {
            var type = (MatterType)Enum.Parse(typeof(MatterType), info["matter_type"]);

            switch (type)
            {
                case MatterType.oneOffEvent:
                    return Json.Import<string, string>(info).ConvertTo<MatterOneOff>();
                case MatterType.periodicEvent:
                    return Json.Import<string, string>(info).ConvertTo<MatterPeriodicity>();
                case MatterType.email:
                    return Json.Import<string, string>(info).ConvertTo<MatterEmail>();

            }
            return null;
        }
        public static MatterBasic ParseFromJson(Json info)
        {
            var type = (MatterType)Enum.Parse(typeof(MatterType), info["matter_type"]);

            switch (type)
            {
                case MatterType.oneOffEvent:
                    return info.ConvertTo<MatterOneOff>();
                case MatterType.periodicEvent:
                    return info.ConvertTo<MatterPeriodicity>();
                case MatterType.email:
                    return info.ConvertTo<MatterEmail>();

            }
            return null;
        }
        protected static void CopyTo(MatterBasic source,MatterBasic target)
        {
            target.user_id = source.user_id;
            target.matter_desc = source.matter_desc;
            target.matter_addion_info = source.matter_addion_info;
            target.matter_end_time = source.matter_end_time;
            target.matter_type = source.matter_type;
            target.matter_id = source.matter_id;
            target.matter_name = source.matter_name;
            target.matter_start_time = source.matter_start_time;
            target.matter_next_effect_time = source.matter_next_effect_time;
            target.folder_id = source.folder_id;
            target.is_new = source.is_new;
        }
        /// <summary>
        /// 用于服务器上定时执行任务
        /// </summary>
        public void Tick()
        {

        }
        /// <summary>
        /// 返回事件是否结束
        /// </summary>
        public virtual bool OnUserFinish()
        {
            return false;
        }
    }
}
