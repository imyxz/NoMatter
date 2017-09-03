using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;

namespace CommonClass
{
    public class MatterOneOff : MatterBasic
    {
        public MatterOneOff():base()
        {
            matter_type = MatterType.oneOffEvent;
        }
        public override Object fromJson(Json json)
        {
            return json.ConvertTo<MatterOneOff>((string c, string m) =>
            {
                switch (m)
                {
                    case "is_new":
                        return false;
                }
                return true;
            });
        }

        public override DateTime GetNextEffectTime()
        {
            throw new NotImplementedException();
        }
        public static MatterOneOff converToMe(MatterBasic matter)
        {
            var ret = new MatterOneOff();
            MatterBasic.CopyTo(matter, ret);
            ret.matter_addion_info = new Json();
            ret.matter_type = MatterType.oneOffEvent;
            return ret;
        }
        public override bool OnUserFinish()
        {
            return true;
        }
    }
}