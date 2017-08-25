using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON
{
    internal enum JSONDecodeStatus
    {
        UNKNOWN,
        DICTIONARY,
        ARRAY,
        INT,
        DOUBLE,
        BOOL,
        STRING
    }
}