using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
namespace CommonClass
{
    public class MatterPeriodicity : MatterBasic
    {
        public MatterPeriodicity():base()
        {
            matter_type = MatterType.periodicEvent;
        }
        public MatterPeriodicityType period_type = MatterPeriodicityType.OneTime;
        public override DateTime GetNextEffectTime()
        {
            throw new NotImplementedException();
        }
        public override Object fromJson(Json json)
        {
            var tmp= json.ConvertTo<MatterPeriodicity>((string c, string m) =>
            {
                switch (m)
                {
                    case "is_new":
                        return false;
                }
                return true;
            });
            if (!tmp.matter_addion_info["period_type"].IsNull())
                tmp.period_type = (MatterPeriodicityType)Enum.Parse(typeof(MatterPeriodicityType), (string)tmp.matter_addion_info["period_type"]);
            else
                tmp.period_type = MatterPeriodicityType.OneTime;
            return tmp;
        }
        public override Json toJson()
        {
            this.matter_addion_info["period_type"] = period_type.ToString();
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
        public static MatterPeriodicity converToMe(MatterBasic matter,MatterPeriodicityType period)
        {
            var ret = new MatterPeriodicity();
            MatterBasic.CopyTo(matter, ret);
            ret.matter_addion_info = new Json();
            ret.period_type = period;
            ret.matter_type = MatterType.periodicEvent;
            return ret;
        }
        public override bool OnUserFinish()
        {
            var tmp = matter_next_effect_time;
            
            switch (period_type)
            {
                case MatterPeriodicityType.OneTime:
                    return true;
                case MatterPeriodicityType.Dayly:
                    tmp = tmp.AddDays(1);
                    break;
                case MatterPeriodicityType.Weekly:
                    tmp = tmp.AddDays(7);
                    break;
                case MatterPeriodicityType.Monthly:
                    tmp = tmp.AddMonths(1);
                    break;
                default:
                    return true;
            }
            matter_next_effect_time = tmp;
            if (tmp > matter_end_time)
                return true;
            else
                return false;
        }
    }
}