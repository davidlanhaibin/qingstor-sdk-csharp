using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;

namespace QingStor_SDK_CSharp.Common
{
    // 配置类
    public class CConfig
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Protoco { get; set; }
        public Int32 ConnectionRetry { get; set; }
        public string LogLevel { get; set; }
        public string AccessKeyID { get; set; }
        public string SecretAccessKey { get; set; }

        public CConfig(string strAccessKeyID, string strSecretAccessKey)
        {
            this.AccessKeyID = strAccessKeyID;
            this.SecretAccessKey = strSecretAccessKey;
            this.Host = "www.qingstor.com";
            this.Port = "";
            this.Protoco = "http";
            this.ConnectionRetry = 5;
            this.LogLevel = "warning";
        }

        public CConfig(string strConfigFile)
        {
            if (!LoadConfigFile(strConfigFile))
            {
                this.AccessKeyID = "";
                this.SecretAccessKey = "";
                this.Host = "www.qingstor.com";
                this.Port = "";
                this.Protoco = "http";
                this.ConnectionRetry = 5;
                this.LogLevel = "warning";
            }
        }
        
        public CConfig()
        {
            this.AccessKeyID = "";
            this.SecretAccessKey = "";
            this.Host = "www.qingstor.com";
            this.Port = "";
            this.Protoco = "http";
            this.ConnectionRetry = 5;
            this.LogLevel = "warning";
        }

        ~CConfig()
        { 
        }

        private bool LoadConfigFile(string strConfigFile)
        {
            if (File.Exists(strConfigFile))
            {
                try
                {
                    // Json串
                    StreamReader SR = new StreamReader(strConfigFile);
                    string strLine = "";
                    string strJson = "";
                    while ((strLine = SR.ReadLine()) != null)
                    {
                        strJson += strLine;
                    }

                    // 将Json串转换为Config结构
                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    if (Serializer == null)
                    {
                        return false;
                    }
                    CConfig Config = Serializer.Deserialize<CConfig>(strJson);
                    this.Host = Config.Host;
                    this.Port = Config.Port;
                    this.Protoco = Config.Protoco;
                    this.ConnectionRetry = Config.ConnectionRetry;
                    this.LogLevel = Config.LogLevel;
                    this.AccessKeyID = Config.AccessKeyID;
                    this.SecretAccessKey = Config.SecretAccessKey;

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
