using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;

namespace CommonClass
{
    public class MatterOneOff : MatterBasic
    {
        public override Object fromJson(Json json)
        {
            return json.ConvertTo<MatterOneOff>((string c, string m) =>
            {
                /*switch (m)
                {
                    case "PassWord":
                        return false;
                }*/
                return true;
            });
        }

        public override DateTime GetNextEffectTime()
        {
            throw new NotImplementedException();
        }
    }
}