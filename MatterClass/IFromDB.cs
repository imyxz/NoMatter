using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass
{
    interface IFromDB
    {
        object ParseFromDB(Dictionary<string, string> info);
    }
}
