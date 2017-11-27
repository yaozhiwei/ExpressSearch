using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;

namespace ExpressSearch.Code
{
    public static class Json
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            //不循环引用，否则会导致死循环             
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //日期类型默认格式化处理  
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json, settings);
        }
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }
        public static string ToJson(this object obj, string datetimeformats)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }
}
