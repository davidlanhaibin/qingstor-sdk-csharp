using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using QingStor_SDK_CSharp.Common;

namespace QingStor_SDK_CSharp.Request
{
    // 参数类型
    enum EParamType
    {
        PARAM_TYPE_HEADER,  // header
        PARAM_TYPE_QUERY,   // query
        PARAM_TYPE_BODY     // body
    };

    // Http请求类
    public class CRequest
    {
        private static CRequest SingletonInstance = null;

        private CRequest()
        {

        }
        ~CRequest()
        {

        }

        public static CRequest GetInstance()
        {
            if (SingletonInstance == null)
            {
                SingletonInstance = new CRequest();
            }

            return SingletonInstance;
        }

        // Http请求入口
        public string Request(Dictionary<Object, Object> DictInput, Object InputParam)
        {
            string strResponse = "{";
            try
            {
                HttpWebRequest HttpRequest = CreateRequest(DictInput, InputParam);
                if (HttpRequest != null)
                {
                    HttpWebResponse HttpResponse = HttpRequest.GetResponse() as HttpWebResponse;
                    if (HttpResponse != null)
                    {
                        // StatusCode,RequestID
                        strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\"", 
                                                    (int)HttpResponse.StatusCode, 
                                                    HttpResponse.Headers.Get("X-QS-Request-ID"));

                        // Headers
                        string[] strHeaders = HttpResponse.Headers.AllKeys;
                        foreach (var Item in strHeaders)
                        {
                            string strKey = Item.ToString();
                            strKey = strKey.Replace("-", "_");
                            string strValue = HttpResponse.Headers.Get(Item.ToString());
                            strValue = strValue.Replace("\"", "");
                            strResponse += string.Format(", \"{0}\":\"{1}\"", strKey, strValue);
                        }

                        // Body
                        if (HttpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            Stream ResponseStream = HttpResponse.GetResponseStream();
                            if (ResponseStream != null)
                            {
                                StreamReader Reader = new StreamReader(ResponseStream);
                                if (Reader != null)
                                {
                                    string strStream = Reader.ReadToEnd();
                                    if (strStream.StartsWith("{") && strStream.EndsWith("}") && strStream.Length > 2)
                                    {
                                        strStream = strStream.Substring(1, strStream.Length - 2);
                                        strResponse += string.Format(", {0}", strStream);
                                    }
                                }
                            }
                        }  // if (HttpResponse.StatusCode == HttpStatusCode.OK)

                        strResponse += "}";
                        return strResponse;
                    }
                }  // if (HttpRequest != null)

                strResponse += string.Format("\"StatusCode\":{0}", -1);
                strResponse += "}";
                return strResponse;
            }
            catch (WebException e)
            {
                // 异常信息
                HttpWebResponse HttpResponse = e.Response as HttpWebResponse;
                Stream Stream = e.Response.GetResponseStream();
                StreamReader Reader = new StreamReader(Stream);
                string strException = Reader.ReadToEnd();
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                CExceptionInfo ExceptionInfo = Serializer.Deserialize<CExceptionInfo>(strException);

                if (ExceptionInfo != null)
                {
                    strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\", \"ErrorCode\":\"{2}\"",
                                                 (int)HttpResponse.StatusCode, ExceptionInfo.request_id, ExceptionInfo.code);
                }
                else
                {
                    strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\", \"ErrorCode\":\"{2}\"",
                                                 (int)HttpResponse.StatusCode, "", "");
                }
                strResponse += "}";

                return strResponse;
            }
        }

        // Http请求入口,输出Body对象
        public string Request(Dictionary<Object, Object> DictInput, Object InputParam, ref Object ObjOutput)
        {
            string strResponse = "{";
            try
            {
                HttpWebRequest HttpRequest = CreateRequest(DictInput, InputParam);
                if (HttpRequest != null)
                {
                    HttpWebResponse HttpResponse = HttpRequest.GetResponse() as HttpWebResponse;
                    if (HttpResponse != null)
                    {
                        // StatusCode,RequestID
                        strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\"",
                                                    (int)HttpResponse.StatusCode,
                                                    HttpResponse.Headers.Get("X-QS-Request-ID"));

                        // Headers
                        string[] strHeaders = HttpResponse.Headers.AllKeys;
                        foreach (var Item in strHeaders)
                        {
                            string strKey = Item.ToString();
                            strKey = strKey.Replace("-", "_");
                            string strValue = HttpResponse.Headers.Get(Item.ToString());
                            strValue = strValue.Replace("\"", "");
                            strResponse += string.Format(", \"{0}\":\"{1}\"", strKey, strValue);
                        }

                        // Body
                        if (HttpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            Stream ResponseStream = HttpResponse.GetResponseStream();
                            if (ResponseStream != null)
                            {
                                ObjOutput = new StreamReader(ResponseStream);
                            }
                        }

                        strResponse += "}";
                        return strResponse;
                    }
                }  // if (HttpRequest != null)

                strResponse += string.Format("\"StatusCode\":{0}", -1);
                strResponse += "}";
                return strResponse;
            }
            catch (WebException e)
            {
                // 异常信息
                HttpWebResponse HttpResponse = e.Response as HttpWebResponse;
                Stream Stream = e.Response.GetResponseStream();
                StreamReader Reader = new StreamReader(Stream);
                string strException = Reader.ReadToEnd();
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                CExceptionInfo ExceptionInfo = Serializer.Deserialize<CExceptionInfo>(strException);

                if (ExceptionInfo != null)
                {
                    strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\", \"ErrorCode\":\"{2}\"",
                                                 (int)HttpResponse.StatusCode, ExceptionInfo.request_id, ExceptionInfo.code);
                }
                else
                {
                    strResponse += string.Format("\"StatusCode\":{0}, \"RequestID\":\"{1}\", \"ErrorCode\":\"{2}\"",
                                                 (int)HttpResponse.StatusCode, "", "");
                }
                strResponse += "}";

                return strResponse;
            }
        }

        // 构建WebHttpRequest
        private HttpWebRequest CreateRequest(Dictionary<Object, Object> dictInput, Object InputParam)
        {
            try
            {
                // 分别提取Header、Query、Body三种类型的参数
                Dictionary<EParamType, Dictionary<Object, Object>> dictParams = GetParams(InputParam);
                if (dictParams == null)
                {
                    return null;
                }
                Dictionary<Object, Object> dictHeaderParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_HEADER) ? 
                                                              dictParams[EParamType.PARAM_TYPE_HEADER] : null;
                Dictionary<Object, Object> dictQueryParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_QUERY) ? 
                                                             dictParams[EParamType.PARAM_TYPE_QUERY] : null;
                Dictionary<Object, Object> dictBodyParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_BODY) ?
                                                            dictParams[EParamType.PARAM_TYPE_BODY] : null;
                
                HttpWebRequest HttpRequest = HttpWebRequest.Create(CreateURL(dictInput, dictQueryParams)) as HttpWebRequest;
                if (HttpRequest == null)
                {
                    return null;
                }

                // 方法
                string strMethod = dictInput.ContainsKey(ConstDef.REQ_HEADER_METHOD) ?
                                   dictInput[ConstDef.REQ_HEADER_METHOD].ToString().ToUpper() : "";
                HttpRequest.Method = strMethod;

                // 添加Body
                if ((!strMethod.Equals("GET") && !strMethod.Equals("HEAD")) && !AddBody(dictParams, ref HttpRequest))
                {
                    //
                }

                // 添加Http头部信息
                if (!AddHeadersInfo(dictInput, dictHeaderParams, ref HttpRequest))
                {
                    //
                }

                // Authorization
                CAuthorization Auth = new CAuthorization(HttpRequest, 
                                                         dictInput[ConstDef.REQ_HEADER_ACCESS_KEY_ID].ToString(),
                                                         dictInput[ConstDef.REQ_HEADER_SECRET_ACCESS_KEY].ToString());
                if (Auth == null)
                {
                    return null;
                }
                HttpRequest.Headers.Add("Authorization", Auth.GetAuthorization());

                return HttpRequest;
            }
            catch (WebException e)
            {
                return null;
            }
        }

        // 构建URL串
        private string CreateURL(Dictionary<Object, Object> dictInput, Dictionary<Object, Object> dictQueryParams)
        {
            try
            {
                string strURL = "";

                // 去掉Host中的"www."
                string strHost = dictInput.ContainsKey(ConstDef.REQ_HEADER_HOST) ? dictInput[ConstDef.REQ_HEADER_HOST].ToString() : "";
                strHost = strHost.ToLower();
                if (strHost.StartsWith("www."))
                {
                    strHost = strHost.Substring(4);
                }

                // 端口为空
                if (dictInput.ContainsKey(ConstDef.REQ_HEADER_PORT)
                    && dictInput[ConstDef.REQ_HEADER_PORT].ToString().Equals(""))
                {
                    strURL = string.Format("{0}", strHost);
                }
                else
                {
                    strURL = string.Format("{0}:{1}", strHost, dictInput[ConstDef.REQ_HEADER_PORT]);
                }

                // 存在Zone
                if (dictInput.ContainsKey(ConstDef.REQ_HEADER_ZONE)
                    && !dictInput[ConstDef.REQ_HEADER_ZONE].ToString().Equals(""))
                {
                    strURL = string.Format("{0}.", dictInput[ConstDef.REQ_HEADER_ZONE]) + strURL;
                }

                // 协议
                string strProtoco = dictInput.ContainsKey(ConstDef.REQ_HEADER_PROTOCO) ?
                                    dictInput[ConstDef.REQ_HEADER_PROTOCO].ToString() : "http";
                strURL = string.Format("{0}://", strProtoco) + strURL;

                // 路径
                string strURLPath = dictInput.ContainsKey(ConstDef.REQ_HEADER_REQUEST_PATH) ?
                                    dictInput[ConstDef.REQ_HEADER_REQUEST_PATH].ToString() : "";
                strURL += strURLPath;

                // GET方法或子资源将参数追加到URL末尾
                string strMethod = dictInput.ContainsKey(ConstDef.REQ_HEADER_METHOD) ?
                                   dictInput[ConstDef.REQ_HEADER_METHOD].ToString().ToUpper() : "";
                if (dictQueryParams != null)
                {
                    // GET方法
                    if (strMethod.Equals("GET"))
                    {
                        // 查询参数
                        string strQueryParams = "";
                        bool bFirst = true;
                        foreach (var Item in dictQueryParams)
                        {
                            if (bFirst)
                            {
                                bFirst = false;
                                strQueryParams += string.Format("{0}={1}", Item.Key, Item.Value);
                            }
                            else
                            {
                                strQueryParams += string.Format("&{0}={1}", Item.Key, Item.Value);
                            }
                        }

                        strURL = strURL + (strQueryParams.Equals("") ? "" : string.Format("?{0}", strQueryParams));
                    }  // if (strMethod.Equals("GET"))
                    // 子资源
                    else
                    {
                        // 查询参数
                        string strQueryParams = "";
                        bool bFirst = true;
                        foreach (var Item in dictQueryParams)
                        {
                            if (!CGlobalSet.SetSubResources.Contains(Item.Key))
                            {
                                continue;
                            }

                            if (bFirst)
                            {
                                bFirst = false;
                                strQueryParams += string.Format("{0}={1}", Item.Key, Item.Value);
                            }
                            else
                            {
                                strQueryParams += string.Format("&{0}={1}", Item.Key, Item.Value);
                            }
                        }

                        strURL = strURL + (strQueryParams.Equals("") ? "" : string.Format("?{0}", strQueryParams));
                    }
                }  // if (dictQueryParams != null)

                // URL编码
                //strURL = System.Web.HttpUtility.UrlEncode(strURL);

                return strURL;
            }  // try
            catch (WebException e)
            {
                // 异常信息
                Stream Stream = e.Response.GetResponseStream();
                StreamReader Reader = new StreamReader(Stream);
                string strException = Reader.ReadToEnd();

                return "";
            }
        }

        // 添加Http头部信息
        private bool AddHeadersInfo(Dictionary<Object, Object> dictInput, Dictionary<Object, Object> dictHeaderParams, 
                                    ref HttpWebRequest HttpRequest)
        {
            // 公共头部
            HttpRequest.ProtocolVersion = HttpVersion.Version11;
            HttpRequest.Method = dictInput[ConstDef.REQ_HEADER_METHOD].ToString();
            HttpRequest.Date = DateTime.Now;

            // 添加Header参数
            if (!AddHeaderParams(dictHeaderParams, ref HttpRequest))
            {
                return false;
            }

            return true;
        }

        // 添加Header参数
        private bool AddHeaderParams(Dictionary<Object, Object> dictHeaderParams, ref HttpWebRequest HttpRequest)
        {
            if (dictHeaderParams == null)
            {
                return false;
            }

            // header
            foreach (var Item in dictHeaderParams)
            {
                // Content-Length、Method在前面已经设置完成
                if (Item.Key.ToString().Equals("Content-Length")
                    || Item.Key.ToString().Equals("Method"))
                {
                    continue;
                }

                // Content-Type
                if (Item.Key.ToString().Equals("Content-Type"))
                {
                    HttpRequest.ContentType = Item.Value.ToString();
                }
                // IfModifiedSince
                else if (Item.Key.ToString().Equals("If-Modified-Since"))
                {
                    if (!string.IsNullOrEmpty(Item.Value.ToString()))
                    {
                        HttpRequest.IfModifiedSince = DateTime.Parse(Item.Value.ToString());
                    }
                }
                // Range
                else if (Item.Key.ToString().Equals("Range"))
                {
                    if (!string.IsNullOrEmpty(Item.Value.ToString()))
                    {
                        HttpRequest.AddRange(int.Parse(Item.Value.ToString()));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Item.Value.ToString()))
                    {
                        HttpRequest.Headers.Add(Item.Key.ToString(), Item.Value.ToString());
                    }
                }
            }
            
            return true;
        }

        // 分别提取Header、Query、Body三种类型的参数
        private Dictionary<EParamType, Dictionary<Object, Object>> GetParams(Object InputParam)
        {
            Dictionary<EParamType, Dictionary<Object, Object>> dictParams = new Dictionary<EParamType, Dictionary<Object, Object>>();
            Dictionary<Object, Object> dictHeaderParams = new Dictionary<object, object>();
            Dictionary<Object, Object> dictQueryParams = new Dictionary<object, object>();
            Dictionary<Object, Object> dictBodyParams = new Dictionary<object, object>();

            if (InputParam == null)
            {
                return dictParams;
            }

            Type type = InputParam.GetType();
            PropertyInfo[] aPropertyInfo = type.GetProperties();
            foreach (var Item in aPropertyInfo)
            {
                // 获取属性的描述  
                Object[] aObject = Item.GetCustomAttributes(typeof(CParamAttribute), false);
                foreach (CParamAttribute Attribute in aObject)
                {
                    // 参数值
                    PropertyInfo ProInfo = type.GetProperty(Attribute.ParamName);
                    string strParamKey = CGlobalSet.SetUnderlineID.Contains(Attribute.ParamName) ?
                                         Attribute.ParamName : Attribute.ParamName.Replace("_", "-");
                    Object ParamValue = ProInfo.GetValue(InputParam);

                    // header
                    if (Attribute.ParamType.Equals("header"))
                    {
                        dictHeaderParams.Add(strParamKey, ParamValue);
                    }
                    // query
                    else if (Attribute.ParamType.Equals("query"))
                    {
                        dictQueryParams.Add(strParamKey, ParamValue);
                    }
                    // body
                    else if (Attribute.ParamType.Equals("body"))
                    {
                        dictBodyParams.Add(strParamKey, ParamValue);
                    }
                }  // foreach (CParamAttribute Attribute in aObject)
            }  // foreach (var Item in aPropertyInfo)      

            // 添加到总字典中
            dictParams.Add(EParamType.PARAM_TYPE_HEADER, dictHeaderParams);
            dictParams.Add(EParamType.PARAM_TYPE_QUERY, dictQueryParams);
            dictParams.Add(EParamType.PARAM_TYPE_BODY, dictBodyParams);

            return dictParams;
        }

        // 添加Body
        private bool AddBody(Dictionary<EParamType, Dictionary<Object, Object>> dictParams, 
                             ref HttpWebRequest HttpRequest)
        {
            if (dictParams == null)
            {
                return false;
            }

            Dictionary<Object, Object> dictHeaderParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_HEADER) ? 
                                                          dictParams[EParamType.PARAM_TYPE_HEADER] : null;
            Dictionary<Object, Object> dictQueryParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_QUERY) ? 
                                                         dictParams[EParamType.PARAM_TYPE_QUERY] : null;
            Dictionary<Object, Object> dictBodyParams = dictParams.ContainsKey(EParamType.PARAM_TYPE_BODY) ? 
                                                        dictParams[EParamType.PARAM_TYPE_BODY] : null;

            bool bFirst = true;
            string strBodyData = "";

            // 查询参数
            if (dictQueryParams != null)
            {
                foreach (var Item in dictQueryParams)
                {
                    if (Item.Key != null && Item.Value != null)
                    {
                        // 如果是子资源，则已经添加到了URL末尾
                        if (CGlobalSet.SetSubResources.Contains(Item.Key))
                        {
                            continue;
                        }

                        if (bFirst)
                        {
                            bFirst = false;
                            strBodyData += string.Format("\"{0}\":{1}", Item.Key, CJsonUtils.ObjectToJson(Item.Value));
                        }
                        else
                        {
                            strBodyData += string.Format(", \"{0}\":{1}", Item.Key, CJsonUtils.ObjectToJson(Item.Value));
                        }
                    }
                }
            }

            // 消息体
            FileStream fileStream = null;
            if (dictBodyParams != null)
            {
                foreach (var Item in dictBodyParams)
                {
                    if (Item.Key != null && Item.Value != null)
                    {
                        // 上传文件，需对文件进行特殊处理
                        if (Item.Key.ToString().Equals("Body"))
                        {
                            fileStream = Item.Value as FileStream;
                            continue;
                        }

                        if (bFirst)
                        {
                            bFirst = false;
                            strBodyData += string.Format("\"{0}\":{1}", Item.Key, CJsonUtils.ObjectToJson(Item.Value));
                        }
                        else
                        {
                            strBodyData += string.Format(", \"{0}\":{1}", Item.Key, CJsonUtils.ObjectToJson(Item.Value));
                        }
                    }
                }
            }

            // 是否需要上传文件
            if (fileStream != null)
            {
                if (dictHeaderParams != null)
                {
                    // Content-Type
                    if (dictHeaderParams.ContainsKey(ConstDef.REQ_HEADER_CONTENT_TYPE))
                    {
                        string strContentType = dictHeaderParams[ConstDef.REQ_HEADER_CONTENT_TYPE].ToString();
                        HttpRequest.ContentType = strContentType.Equals("") ? "application/octet-stream" : strContentType;
                    }

                    // Content-Length
                    if (dictHeaderParams.ContainsKey(ConstDef.REQ_HEADER_CONTENT_LENGTH))
                    {
                        HttpRequest.ContentLength = fileStream.Length;
                    }
                }

                byte[] Buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int ReadBytes = 0;
                while ((ReadBytes = fileStream.Read(Buffer, 0, Buffer.Length)) != 0)
                {
                    HttpRequest.GetRequestStream().Write(Buffer, 0, ReadBytes);
                }
            }  // if (fileStream != null)
            else
            {
                strBodyData = "{" + strBodyData + "}";
                byte[] BodyByte = Encoding.UTF8.GetBytes(strBodyData);

                // 设置头部信息
                if (dictHeaderParams != null)
                {
                    // Content-MD5
                    //if (dictHeaderParams.ContainsKey(ConstDef.REQ_HEADER_CONTENT_MD5))
                    //{
                    //    MD5 md5 = new MD5CryptoServiceProvider();
                    //    byte[] ByteMD5 = md5.ComputeHash(BodyByte);
                    //    HttpRequest.Headers.Add(ConstDef.REQ_HEADER_CONTENT_MD5, Convert.ToBase64String(ByteMD5));
                    //}

                    // Content-Type
                    if (dictHeaderParams.ContainsKey(ConstDef.REQ_HEADER_CONTENT_TYPE))
                    {
                        string strContentType = dictHeaderParams[ConstDef.REQ_HEADER_CONTENT_TYPE].ToString();
                        HttpRequest.ContentType = strContentType.Equals("") ? "application/octet-stream" : strContentType;
                    }

                    // Content-Length
                    if (dictHeaderParams.ContainsKey(ConstDef.REQ_HEADER_CONTENT_LENGTH))
                    {
                        HttpRequest.ContentLength = BodyByte.Length;
                    }
                }

                // 写入数据流
                HttpRequest.GetRequestStream().Write(BodyByte, 0, BodyByte.Length);
            }

            return true;
        }
    }
}
