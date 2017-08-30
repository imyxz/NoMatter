﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ServerLib
{
    public interface IController
    {
        Dictionary<string, Func<HttpServerContext, string>> GetActions();
        IController InitController();
        void EndController();
        
        MySqlConnection GetDBConnection();
    }
}
