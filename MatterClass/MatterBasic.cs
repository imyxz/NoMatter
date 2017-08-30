using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass
{
    public abstract class MatterBasic
    {
        public MatterType MatterType
        {
            get => default(MatterType);
            set
            {
            }
        }

        public abstract new int GetID();

        public abstract new MatterType GetType();

        public abstract new string GetName();

        public abstract new string GetDesc();

        public abstract new DateTime GetStartTime();

        public abstract new DateTime GetNextEffectTime();

        public abstract new DateTime GetEndTime();

        public abstract new int GetUserID();
    }
}
