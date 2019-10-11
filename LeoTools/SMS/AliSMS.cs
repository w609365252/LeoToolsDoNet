using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using LeoTools.DataTypeExtend;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.SMS
{
    internal class AliSMS
    {
        internal static bool GetSMSCode(string phone,string code,out string errorMsg)
        {
            try
            {
                string appid = ConfigHelper.GetConfigString("AliSMSAppId");
                string sercert = ConfigHelper.GetConfigString("AliSMSAppSercert");
                string signName = ConfigHelper.GetConfigString("AliSMSSignName");
                IClientProfile profile = DefaultProfile.GetProfile("default", appid, sercert);
                DefaultAcsClient client = new DefaultAcsClient(profile);
                CommonRequest request = new CommonRequest();
                request.Method = Aliyun.Acs.Core.Http.MethodType.POST;
                request.Domain = "dysmsapi.aliyuncs.com";
                request.Version = "2017-05-25";
                request.Action = "SendSms";
                request.AddQueryParameters("PhoneNumbers", phone);
                request.AddQueryParameters("SignName", signName);
                request.AddQueryParameters("TemplateCode", "SMS_166476657");
                request.AddQueryParameters("TemplateParam", "{code : " + code + "}");
                CommonResponse response = client.GetCommonResponse(request);
                //{"Message":"OK","RequestId":"4D194491-23D0-482E-B35C-627FE3E2C708","BizId":"577511859112322204^0","Code":"OK"}
                JObject json = JObject.Parse(response.Data);
                //短信发送成功
                bool flag = false;
                if (json["Message"].ToString() == "OK")
                {
                    flag = true;
                }
                //短信发送过于贫乏
                else if (json["Message"].ToString().Contains("分钟") || json["Message"].ToString().Contains("小时") || json["Message"].ToString().Contains("天") || json["Message"].ToString().Contains("日"))
                {
                    
                }
                errorMsg = json["Message"]?.ToString() ?? "";
                return flag;
            }
            catch (Aliyun.Acs.Core.Exceptions.ServerException ex)
            {
                errorMsg = ex.ToString();
                return false;
            }
        }
    }
}
