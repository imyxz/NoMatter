using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
namespace CommonClass
{
    public class MatterPeriodicity : MatterBasic
    {
        public override DateTime GetNextEffectTime()
        {
            throw new NotImplementedException();
        }
        public override Object fromJson(Json json)
        {
            return json.ConvertTo<MatterPeriodicity>((string c, string m) =>
            {
                /*switch (m)
                {
                    case "PassWord":
                        return false;
                }*/
                return true;
            });
        }
    }
}