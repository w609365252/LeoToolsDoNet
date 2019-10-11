using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using LeoTools.DataTypeExtend;
using LeoTools.NLog;
using static System.Configuration.ConfigurationManager;
namespace LeoTools.OSS
{
    /// <summary>
    /// OSS操作
    /// </summary>
    public class OSSClientManager
    {
        /// <summary>
        /// 获取是否使用自定义域名（CNAME）新建Client
        /// </summary>
        public bool GetIsCname { get { return this.IsCname; } }

        /// <summary>
        /// 在使用自定义域名（CNAME）新建Client的时候, 获取Client的配置信息对象
        /// </summary>
        public ClientConfiguration GetClientConfiguration { get { return this.Config; } }

        /// <summary>
        /// 阿里云的AccessKeyId
        /// </summary>
        private readonly string AccessKeyId;

        /// <summary>
        /// 阿里云的AccessKeySecret
        /// </summary>
        private readonly string AccessKeySecret;

        /// <summary>
        /// 阿里云endpoint地址
        /// </summary>
        private readonly string Endpoint;

        /// <summary>
        /// 阿里云OSS 文件返回地址
        /// </summary>
        private readonly string OSSAddress;

        /// <summary>
        /// 是否使用自定义域名（CNAME）新建Client
        /// </summary>
        private readonly bool IsCname;

        /// <summary>
        /// 自定义域名（CNAME）新建Client的配置对象
        /// </summary>
        private readonly ClientConfiguration Config;

        /// <summary>
        /// OSSClient
        /// </summary>
        private readonly OssClient Client;

        /// <summary>
        /// OSSClient构造函数, 初始化参数用
        /// </summary>
        public OSSClientManager()
        {
            this.AccessKeyId = AppSettings["OSSAccessKeyID"];
            this.AccessKeySecret = AppSettings["OSSAccessKeySecret"];
            this.Endpoint = AppSettings["OSSEndpoint"];
            this.OSSAddress = AppSettings["OSSAddress"];
            this.Client = new OssClient(this.Endpoint, this.AccessKeyId, this.AccessKeySecret);
        }

        /// <summary>
        /// OSSClient构造函数, 初始化参数用
        /// </summary>
        /// <param name="isCname">(该值只能赋true, 否则异常)是否使用自定义域名（CNAME）新建Client, 默认不是  PS:使用CNAME时，无法使用ListBuckets接口</param>
        /// <param name="maxErrorRetry">请求发生错误时最大的重试次数</param>
        /// <param name="connectionTimeout">连接超时时间</param>
        /// <param name="customEpochTicks">
        /// 自定义基准时间。
        /// 
        /// 由于OSS的token校验是时间相关的，可能会因为终端系统时间不准导致无法访问OSS服务。 通过该接口设置自定义Epoch秒数，SDK计算出本机当前时间与自定义时间的差值，之后的每次请求时间均加上该差值，以达到时间校验的目的。
        /// </param>
        public OSSClientManager(bool isCname, int maxErrorRetry, int connectionTimeout, long customEpochTicks)
        {
            if (!isCname) throw new Exception("isCname参数的值只能是True, 如果是False, 请使用无参的构造函数进行初始化");
            this.AccessKeyId = AppSettings["OSSAccessKeyID"];
            this.AccessKeySecret = AppSettings["OSSAccessKeySecret"];
            this.Endpoint = AppSettings["OSSEndpoint"];
            this.IsCname = isCname;
            this.Config = new ClientConfiguration
            {
                ConnectionTimeout = connectionTimeout,
                MaxErrorRetry = maxErrorRetry,
                IsCname = true
            };
            this.Config.SetCustomEpochTicks(customEpochTicks);
            this.Client = new OssClient(this.Endpoint, this.AccessKeyId, this.AccessKeySecret, this.Config);
        }

        /// <summary>
        /// 创建一个新的存储空间(Bucket)
        /// 不要平凡的创建存储空间
        /// </summary>
        /// <param name="bucketName">存储控件的名称,该名称一定要唯一</param>
        /// <param name="bucketType">存储空间类型</param>
        /// <returns>创建成功的话, 返回Bucket对象, 否则返回null</returns>
        public Bucket CreateBucket(string bucketName, BucketType bucketType)
        {
            if (bucketName.IsNullOrWhiteSpace()) throw new Exception("bucket不能为空");
            try
            {
                var exist = this.BucketExist(bucketName, bucketType);
                if (exist == null) throw new Exception("创建失败");
                if ((bool)exist) throw new Exception("该Bucket已经存在, 请更换名称");
                bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
                return this.Client.CreateBucket(bucketName);
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得所有的存储空间(Bucket)
        /// </summary>
        /// <returns>失败返回null</returns>
        /// <Author>旷丽文</Author>
        public IEnumerable<Bucket> GetListBuckets()
        {
            try
            {
                return this.Client.ListBuckets();
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断存储空间(Bucket)是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="bucketType"></param>
        /// <returns>是否存在的bool, 调用失败返回null</returns>
        /// <Author>旷丽文</Author>
        public bool? BucketExist(string bucketName, BucketType bucketType)
        {
            if (bucketName.IsNullOrWhiteSpace()) throw new Exception("bucket不能为空");
            try
            {
                bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
                return this.Client.DoesBucketExist(bucketName);
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 上传字符串到OSS
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <param name="key">Key</param>
        /// <param name="bucketName">存储空间(Bucket)名称</param>
        /// <param name="bucketType">存储空间(Bucket)类型</param>
        /// <returns></returns>
        /// <Author>旷丽文</Author>
        public PutObjectResult PutString(string value, string key, string bucketName, BucketType bucketType)
        {
            if (value.IsNullOrWhiteSpace()) throw new Exception("字符串不能为空");
            if (key.IsNullOrWhiteSpace()) throw new Exception("Key不能为空");
            if (bucketName.IsNullOrWhiteSpace()) throw new Exception("bucket不能为空");
            value = value.Trim();
            try
            {
                key = key.Trim();
                bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
                byte[] binaryData = Encoding.ASCII.GetBytes(value);
                MemoryStream requestContent = new MemoryStream(binaryData);
                var result = this.Client.PutObject(bucketName, key, requestContent);
                requestContent.Close();
                requestContent.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 上传文件到OSS
        /// </summary>
        /// <param name="file">文件内存流</param>
        /// <param name="key">Key</param>
        /// <param name="bucketName">存储空间(Bucket)名称</param>
        /// <param name="bucketType">存储空间(Bucket)类型</param>
        /// <returns></returns>
        /// <Author>旷丽文</Author>
        public PutObjectResult PutFile(Stream file, string key, string bucketName, BucketType bucketType)
        {
            if (file == null) throw new Exception("文件为空");
            if (key.IsNullOrWhiteSpace()) throw new Exception("Key不能为空");
            if (bucketName.IsNullOrWhiteSpace()) throw new Exception("bucket不能为空");
            try
            {
                key = key.Trim();
                bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
                var result = this.Client.PutObject(bucketName, key, file);
                return result;
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
            finally
            {
                file.Close();
                file.Dispose();
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="bucketName">存储空间(Bucket)名称</param>
        /// <param name="bucketType">存储空间(Bucket)类型</param>
        /// <returns></returns>
        /// <Author>旷丽文</Author>
        public Stream DownFile(string key, string bucketName, BucketType bucketType)
        {
            if (key.IsNullOrWhiteSpace()) throw new Exception("Key不能为空");
            try
            {
                key = key.Trim();
                bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
                var file = this.Client.GetObject(bucketName, key);
                return file.Content;
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得OSS里面文件的网络地址
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="bucketName">存储空间(Bucket)名称</param>
        /// <param name="bucketType">存储空间(Bucket)类型</param>
        /// <returns>返回网络地址</returns>
        /// <Author>旷丽文</Author>
        public string GetOssFileUrl(string key, string bucketName, BucketType bucketType)
        {
            if (key.IsNullOrWhiteSpace()) throw new Exception("Key不能为空");
            key = key.Trim();
            bucketName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{bucketName}".ToLower();
            return $"http://{bucketName}.{this.OSSAddress}/{key}";
        }

        /// <summary>
        /// 设置存储空间的访问权限
        /// </summary>
        /// <param name="buckteName">存储空间的名称</param>
        /// <param name="bucketType">存储空间(Bucket)类型</param>
        public void SetBucketAcl(string buckteName, BucketType bucketType)
        {
            try
            {
                buckteName = $"{AppSettings["OSSBucketName"]}{bucketType.ToString()}{buckteName}".ToLower();
                // 指定Bucket ACL为公共读
                this.Client.SetBucketAcl(buckteName, CannedAccessControlList.PublicRead);
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
            }
        }

        // <summary>
        // 获取存储空间的访问权限
        // </summary>
        // <param name="bucketName">存储空间的名称</param>
        //public void GetBucketAcl(string bucketName)
        //{
        //    try
        //    {
        //        var acl = this.Client.GetBucketAcl(bucketName);
        //        Console.WriteLine("Get bucket ACL success");
        //        foreach (var grant in acl.Grants)
        //        {
        //            Console.WriteLine("获取存储空间权限成功，当前权限:{0}", grant.Permission.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Get bucket ACL failed. {0}", ex.Message);
        //    }
        //}

    }
}