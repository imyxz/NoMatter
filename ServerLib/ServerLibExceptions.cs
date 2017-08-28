using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    class InvalidArguments : Exception
    {
        public InvalidArguments(string a) : base(a)
        {

        }
    }
}
