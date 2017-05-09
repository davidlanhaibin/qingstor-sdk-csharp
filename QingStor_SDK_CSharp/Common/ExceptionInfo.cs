using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingStor_SDK_CSharp.Common
{
    // 异常信息类
    public class CExceptionInfo
    {
        public string url { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string request_id { get; set; }
    }
}
