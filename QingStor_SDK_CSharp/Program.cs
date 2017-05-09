using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using Newtonsoft.Json;
using QingStor_SDK_CSharp.Common;
using QingStor_SDK_CSharp.Service;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1.ListBuckets
            CListBucketsInput input2 = new CListBucketsInput();
            input2.Location = "zw";
            CConfig Config = new CConfig("E:\\qingstor-sdk-c#\\QingStor_SDK_CSharp\\Config\\Config.json");
            CQingStor Service = new CQingStor(Config);
            CListBucketsOutput ListBuckets = Service.ListBuckets(null);

            // 2.List Objects
            CBucket Bucket = Service.Bucket("seafile", "pek3a");
            CListObjectsInput ObjectListInput = new CListObjectsInput() { delimiter = "/", limit = 4, prefix = "OK", marker = "a"};
            CListObjectsOutput ListObject = Bucket.ListObjects(null);
            Console.Write(ListObject.keys.Length);

            // 3.Put Bucket
            CBucket PutBucket = Service.Bucket("testbucket-david2", "pek3a");
            CPutBucketOutput BucketOutput = PutBucket.Put();
            CBucket PutBucket2 = new CBucket(Config, "seafile", "zw");
            CPutBucketOutput BucketOutput2 = PutBucket2.Put();

            // 4.DELETE Bucket
            CBucket DeleteBucket = new CBucket(Config, "testbucket-david", "zw");
            CDeleteBucketOutput BucketDeleteOutput = DeleteBucket.Delete();

            // 5.Delete Multiple Objects
            CBucket DeleteMultiObj = new CBucket(Config, "seafile", "zw");
            CDeleteMultipleObjectsInput DelMultiObjInput = new CDeleteMultipleObjectsInput();
            DelMultiObjInput.quiet = false;
            DelMultiObjInput.objects = ListObject.keys;
            CDeleteMultipleObjectsOutput BucketDeleteMultiOutput = DeleteMultiObj.DeleteMultipleObjects(DelMultiObjInput);

            // 6.HEAD Bucket
            CBucket HeadBucket = new CBucket(Config, "seafile", "pek3a");
            CHeadBucketOutput HeadObjOutput = HeadBucket.Head();

            // 7.GET Bucket Statistics
            CBucket StatisticsBucket = new CBucket(Config, "seafile", "pek3a");
            CGetBucketStatisticsOutput StatisticsOutput = StatisticsBucket.GetStatistics();

            // 8.PUT Bucket ACL
            CGranteeType Grantee = new CGranteeType() { id = "usr-LDNEIwIt", type = "user" };
            CACLType ACL = new CACLType() { grantee = Grantee, permission = "FULL_CONTROL" };
            CPutBucketACLInput PutBucketACLInput = new CPutBucketACLInput();
            PutBucketACLInput.acl = new CACLType[] { ACL };
            CPutBucketACLOutput PutBucketACLOutput = Bucket.PutACL(PutBucketACLInput);

            // 9.GET Bucket ACL
            CGetBucketACLOutput GetACLOutput = Bucket.GetACL();

            // 10.PUT Bucket CORS
            CBucket BucketCors = new CBucket(Config, "seafile", "pek3a");
            CPutBucketCORSInput CorsInput = new CPutBucketCORSInput();
            CCORSRuleType CORSRule = new CCORSRuleType();
            string[] strHeaders = new string[] { "X-QS-Date", "Content-Type", "Content-MD5", "Authorization" };
            CORSRule.allowed_headers = strHeaders;
            string[] strMethods = new string[] { "PUT", "GET", "HEAD"};
            CORSRule.allowed_methods = strMethods;
            CORSRule.allowed_origin = "http://*.qingstorage.com";
            CorsInput.cors_rules = new CCORSRuleType[] {CORSRule };
            CPutBucketCORSOutput CorsOutput = BucketCors.PutCORS(CorsInput);

            // 11.GET Bucket CORS
            CGetBucketCORSOutput GetCorsOutput = BucketCors.GetCORS();

            // 12.DELETE Bucket CORS
            CDeleteBucketCORSOutput DelCorsOutput = BucketCors.DeleteCORS();

            // 13.PUT Bucket Policy
            CBucket BucketPolicy = new CBucket(Config, "csharp", "pek3a");
            CPutBucketPolicyInput PolicyInput = new CPutBucketPolicyInput();
            CStatementType stmt = new CStatementType();
            stmt.id = "allow usr-qo3yay7t to list objects and create objects";
            string[] strUsers = new string[] { "usr-qo3yay7t" };
            stmt.user = strUsers;
            stmt.effect = "deny";
            string[] strResource = new string[] { "csharp/王鹏亮.pdf" };
            stmt.resource = strResource;
            string[] action = new string[] { "list_objects", "create_object" };
            stmt.action = action;
            CConditionType Condition = new CConditionType();
            string[] strRefer = new string[] { "*.pek3a.qingstor.com" };
            Condition.string_like = new CStringLikeType() { Referer = strRefer };
            stmt.condition = Condition;
            PolicyInput.statement = new CStatementType[] { stmt };
            CPutBucketPolicyOutput PolicyOutput = BucketPolicy.PutPolicy(PolicyInput);

            // 14.GET Bucket Policy
            CGetBucketPolicyOutput GetPolicyOutput = BucketPolicy.GetPolicy();

            // 15.DELETE Bucket Policy
            CDeleteBucketPolicyOutput DelPolicyOutput = BucketPolicy.DeletePolicy();

            // 16.PUT Bucket External Mirror
            CBucket BucketMirror = new CBucket(Config, "seafile", "pek3a");
            CPutBucketExternalMirrorInput MirrorInput = new CPutBucketExternalMirrorInput();
            MirrorInput.source_site = "http://download.csdn.net/download";
            CPutBucketExternalMirrorOutput MirrorOutput = BucketMirror.PutExternalMirror(MirrorInput);

            // 17.GET Bucket External Mirror
            CGetBucketExternalMirrorOutput GetMirrorOutput = BucketMirror.GetExternalMirror();

            // 18.DELETE Bucket External Mirror
            CDeleteBucketExternalMirrorOutput DelMirrorOutput = BucketMirror.DeleteExternalMirror();

            // 19.GET Object
            CBucket BucketObj = new CBucket(Config, "csharp", "pek3a");
            CGetObjectInput ObjInput = new CGetObjectInput();
            CGetObjectOutput ObjOutput = BucketObj.GetObject("5.txt", ObjInput);
            StreamReader SR2 = ObjOutput.Body;
            FileStream fs = File.Create("E:\\Win.txt");
            if (SR2 != null)
            {
                string strStream = SR2.ReadToEnd();
                byte[] DataByte = System.Text.Encoding.Default.GetBytes(strStream);
                fs.Write(DataByte, 0, DataByte.Length);
                fs.Flush();
                fs.Close();
            }

            // 20.POST Object Web页面使用,SDK不提供
            // 21.PUT Object
            CBucket BucketPutObj = new CBucket(Config, "csharp", "pek3a");
            CPutObjectInput PutObjInput = new CPutObjectInput();
            PutObjInput.Content_Type = "application/pdf";
            PutObjInput.Body = new FileStream("C:\\Users\\lanhaibin\\Desktop\\侯选人简历\\王鹏亮.pdf", FileMode.Open);
            CPutObjectOutput PutObjOutput = BucketPutObj.PutObject("王鹏亮.pdf", PutObjInput);

            // 22.PUT Object - Copy
            CPutObjectInput PutObjInputCopy = new CPutObjectInput();
            CBucket BucketCopy = new CBucket(Config, "csharp", "pek3a");
            PutObjInputCopy.Content_Type = "image/jpeg";
            PutObjInputCopy.X_QS_Copy_Source = "/seafile/test.JPG";
            CPutObjectOutput PutObjOutputCopy = BucketCopy.PutObject("test", PutObjInputCopy);

            // 23.PUT Object - Move
            CPutObjectInput MoveObjInput = new CPutObjectInput();
            MoveObjInput.Content_Type = "image/jpeg";
            string strMoveSrc = "/seafile/test.JPG";
            strMoveSrc = System.Web.HttpUtility.UrlEncode(strMoveSrc);
            MoveObjInput.X_QS_Move_Source = strMoveSrc;
            CBucket BucketMoveObj = new CBucket(Config, "csharp", "pek3a");
            CPutObjectOutput MoveObjOutput = BucketMoveObj.PutObject("test", MoveObjInput);

            // 24.DELETE Object
            CBucket BucketDelObj = new CBucket(Config, "seafile", "pek3a");
            CDeleteObjectOutput DelObjOutput = BucketObj.DeleteObject("5.txt");

            // 25.HEAD Object
            CBucket BucketHeadObj = new CBucket(Config, "seafile", "pek3a");
            CHeadObjectInput HeadObjInput = new CHeadObjectInput();
            CHeadObjectOutput HeadObjectOutput = BucketObj.HeadObject("test.JPG", HeadObjInput);

            // 26.OPTIONS Object
            COptionsObjectInput OptionsObjInput = new COptionsObjectInput();
            OptionsObjInput.Origin = "http://csharp.pek3a.qingstor.com";
            OptionsObjInput.Access_Control_Request_Method = "GET";
            OptionsObjInput.Access_Control_Request_Headers = "Content-MD5";
            COptionsObjectOutput OptionsObjOutput = BucketObj.OptionsObject("InstallationLog.txt", OptionsObjInput);

            // 27.Initiate Multipart Upload
            CInitiateMultipartUploadInput InitInput = new CInitiateMultipartUploadInput();
            CInitiateMultipartUploadOutput InitOutput = BucketObj.InitiateMultipartUpload("Windows.pdf", InitInput);

            // 28.Upload Multipart
            CUploadMultipartInput UploadInput = new CUploadMultipartInput();
            UploadInput.upload_id = InitOutput.upload_id;
            UploadInput.part_number = 0;
            UploadInput.Body = File.Open("D:\\电子书\\Windows内核安全与驱动开发.pdf", FileMode.Open);
            CUploadMultipartOutput UploadOutput = BucketObj.UploadMultipart("Windows.pdf", UploadInput);

            // 29.List Multipart
            CListMultipartInput ListMultiInput = new CListMultipartInput();
            ListMultiInput.upload_id = InitOutput.upload_id;
            ListMultiInput.limit = 800;
            CListMultipartOutput ListMultiOutput = BucketObj.ListMultipart("Windows.pdf", ListMultiInput);

            // 30.Abort Multipart Upload
            CAbortMultipartUploadInput AbortMultiInput = new CAbortMultipartUploadInput();
            AbortMultiInput.upload_id = InitOutput.upload_id;
            //CAbortMultipartUploadOutput AbortMultiOutput = BucketObj.AbortMultipartUpload("Windows.pdf", AbortMultiInput);

            // 31.Complete Multipart Upload
            CCompleteMultipartUploadInput CompleteInput = new CCompleteMultipartUploadInput();
            CompleteInput.upload_id = InitOutput.upload_id;
            CObjectPartType ObjPart = new CObjectPartType();
            ObjPart.part_number = 0;
            CompleteInput.object_parts = new CObjectPartType[] { ObjPart };
            CCompleteMultipartUploadOutput CompleteOutput = BucketObj.CompleteMultipartUpload("Windows.pdf", CompleteInput);
        }
    }
}
