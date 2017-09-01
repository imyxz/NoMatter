using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON 
{
    public interface IJsonAble
    {
        Json toJson();
        Object fromJson(Json json);
       
    }
}