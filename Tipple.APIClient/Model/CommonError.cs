using System;
using System.Collections.Generic;
using System.Text;

namespace Tipple.APIClient.Model
{
    public class CommonError
    {
        public int Success { get; set; }
        public string Error { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

    }
}
