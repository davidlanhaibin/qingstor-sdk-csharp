.. _qingstor-sdk-c#:

======
C# SDK
======

QingStor C# SDK 已在 GitHub 开源，下文为简要使用文档。更多详细信息请参见
`https://github.com/yunify/qingstor-sdk-csharp <https://github.com/yunify/qingstor-sdk-csharp>`_
。

----
安装
----

可以直接使用 ``go get`` 安装::

    $ go get -u github.com/yunify/qingstor-sdk-csharp

也可以访问 GitHub 的
`release 页面 <https://github.com/yunify/qingstor-sdk-csharp/releases>`_
下载压缩包

-------
快速开始
-------

使用 SDK 之前请先在
`青云控制台 <https://console.qingcloud.com/access_keys/>`_
申请 access key 。

.. rubric:: **初始化服务**

发起请求前首先建立需要初始化服务::

    using QingStor_SDK_CSharp.Common;
    using QingStor_SDK_CSharp.Service;

    CConfig Config = new CConfig("E:\\qingstor-sdk-c#\\QingStor_SDK_CSharp\\Config\\Config.json");
    CQingStor Service = new CQingStor(Config);

上面代码初始化了一个 QingStor Service

.. rubric:: **获取账户下的 Bucket 列表**
::

    CListBucketsOutput ListBuckets = Service.ListBuckets(null);

    // Print the HTTP status code.
    // Example: 200
    Console.Write(ListBuckets.StatusCode);

    // Print the bucket count.
    // Example: 5
    Console.Write(ListBuckets.count);

.. rubric:: **创建 Bucket**

初始化并创建 Bucket, 需要指定 Bucket 名称和所在 Zone::
	
    CBucket Bucket = Service.Bucket("bucket-name", "pek3a");
    CPutBucketOutput PutBucketOutput = Bucket.Put();
			
.. rubric:: **获取 Bucket 中存储的 Object 列表**
::

    CListObjectsOutput ListObjectsOutput = Bucket.ListObjects(null);

    // Print the HTTP status code.
    // Example: 200
    Console.Write(ListObjectsOutput.StatusCode);

    // Print the key count.
    // Example: 0
    Console.Write(ListObjectsOutput.keys.Length);

.. rubric:: **创建一个 Object**
例如上传一张屏幕截图::

    CPutObjectInput PutObjectInput = new CPutObjectInput();
    PutObjInput.Body = new FileStream("/tmp/Screenshot.jpg", FileMode.Open);
    CPutObjectOutput PutObjectOutput = Bucket.PutObject("Screenshot.jpg", PutObjectInput);
			
    // Print the HTTP status code.
    // Example: 201
    Console.Write(PutObjectOutput.StatusCode);

.. rubric:: **删除一个 Object**
::

    CDeleteObjectOutput DeleteObjectOutput = BucketObj.DeleteObject("Screenshot.jpg");

    // Print the HTTP status code.
    // Example: 204
    Console.Write(DeleteObjectOutput.StatusCode);

.. rubric:: **设置 Bucket ACL**
::

    CGranteeType Grantee = new CGranteeType() { id = "usr-LDNEIwIt", type = "user" };
    CACLType ACL = new CACLType() { grantee = Grantee, permission = "FULL_CONTROL" };
    CPutBucketACLInput PutBucketACLInput = new CPutBucketACLInput();
    PutBucketACLInput.acl = new CACLType[] { ACL };
    CPutBucketACLOutput PutBucketACLOutput = Bucket.PutACL(PutBucketACLInput);

    // Print the HTTP status code.
    // Example: 200
    Console.Write(PutBucketACLOutput.StatusCode);
