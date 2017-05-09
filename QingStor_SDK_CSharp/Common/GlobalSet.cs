using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingStor_SDK_CSharp.Common
{
    // 全局公共数据集合类
    public static class CGlobalSet
    {
        // 带下划线的标识符集合
        public static HashSet<string> SetUnderlineID = new HashSet<string>() 
        { 
            "source_site",
            "string_like",
            "string_not_like",
            "ip_address",
            "not_ip_address",
            "is_null",
            "part_number",
            "part_number_marker",
            "upload_id",
            "source_ip",
            "allowed_origin",
            "allowed_methods",
            "allowed_headers",
            "expose_headers",
            "max_age_seconds",
            "mime_type",
            "cors_rules",
            "object_part",
            "object_parts",
            "key_delete_error",
            "next_marker",
            "common_prefixes",
            "access_key_id",
            "common_prefixes"
        };

        // 子资源集合
        public static HashSet<string> SetSubResources = new HashSet<string>() 
        { 
            ConstDef.SUB_RESOURCE_ACL,
            ConstDef.SUB_RESOURCE_CORS,
            ConstDef.SUB_RESOURCE_DELETE,
            ConstDef.SUB_RESOURCE_MIRROR,
            ConstDef.SUB_RESOURCE_POLICY,
            ConstDef.SUB_RESOURCE_STATS,
            ConstDef.SUB_RESOURCE_PART_NUMBER,
            ConstDef.SUB_RESOURCE_UPLOADS,
            ConstDef.SUB_RESOURCE_UPLOAD_ID
        };

        // 响应字段集合
        public static HashSet<string> SetResponseFields = new HashSet<string>() 
        {
            ConstDef.RESPONSE_FIELD_EXPIRES,
            ConstDef.RESPONSE_FIELD_CACHE_CONTROL,
            ConstDef.RESPONSE_FIELD_CONTENT_TYPE,
            ConstDef.RESPONSE_FIELD_CONTENT_LANGUAGE,
            ConstDef.RESPONSE_FIELD_CONTENT_ENCODING,
            ConstDef.RESPONSE_FIELD_CONTENT_DISPOSITION
        };
    }
}
