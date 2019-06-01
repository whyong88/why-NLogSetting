using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NLogSetting.Models
{
    public class ResultData<T> where T : class, new()
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 是否验证失败
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 提示内容
        /// </summary>
        public string Msg { get; set; }

        public void ToSuccess()
        {
            Success = true;
            StatusCode = HttpStatusCode.OK;
            Msg = "操作成功";
            IsValid = true;
        }

        public void ToSuccess(T data)
        {
            ToSuccess();
            Data = data;
        }

        public void ToSuccess(T data, string msg)
        {
            ToSuccess(data);
            Msg = msg;
        }

        public void ToErrValidation(string msg)
        {
            Success = false;
            StatusCode = HttpStatusCode.OK;
            IsValid = false;
            Msg = msg;
        }


        /// <summary>
        /// 返回json格式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            setting.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, setting);
        }
    }

    public class ResultData : ResultData<object>
    {

    }
}