using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class LoginData
    {
        private string userName="";
        private string passWord = "";

        public string UserName { get => userName; set => userName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
    }
}
