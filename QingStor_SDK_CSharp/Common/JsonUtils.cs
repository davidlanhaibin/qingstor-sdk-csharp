using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace QingStor_SDK_CSharp.Common
{
    // Json工具
    public static class CJsonUtils
    {
        // 从一个对象信息生成Json串
        public static string ObjectToJson(object Obj)
        {
            if (Obj == null)
            {
                return "";
            }

            string strJson = JsonConvert.SerializeObject(Obj, Formatting.Indented, 
                                                         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return strJson;
        }

        // 字典对象生成Json串
        public static string DictionaryToJson(Dictionary<Object, Object> DictObj)
        {
            JavaScriptSerializer JSS = new JavaScriptSerializer();

            return JSS.Serialize(DictObj);
        }

        // 从一个Json串生成对象信息
        public static object JsonToObject(string strJson, object Obj)
        {
            if (strJson.Equals(""))
            {
                return null;
            }

            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(Obj.GetType());
            MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(strJson));

            return Serializer.ReadObject(Stream);
        }
    }
}
