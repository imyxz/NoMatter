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
    class InvalidRequest : Exception
    {
        private string errorInfo="";
        private int code=0;

        public InvalidRequest(string a) : base(a)
        {

        }
        public InvalidRequest(int code,string message) : base(message)
        {
            errorInfo = message;
            this.code = code;
        }

        public string ErrorInfo { get => errorInfo; set => errorInfo = value; }
        public int Code { get => code; set => code = value; }
    }
}
