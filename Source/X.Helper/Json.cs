using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

namespace X.Helper
{
    public static class Json
    {

        /// <summary>
        /// 任意对象序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="retain">保留属性</param>
        /// <param name="ignore">忽略属性</param>
        /// <param name="turns">要转换的属性</param>
        /// <param name="ignoreLargeObject">是否过滤超大对象</param>
        /// <param name="largeObjectLengthLimit">超大对象长度标准</param>
        /// <param name="largeObjectIgnoreMask">超大对象过滤后替换文本</param>
        /// <returns>返回数据</returns>
        public static string Serialize(object obj,
            string[] retain = null,
            string[] ignore = null,
            IDictionary<string, string> turns = null,
            bool ignoreLargeObject = false,
            int largeObjectLengthLimit = 500,
            string largeObjectIgnoreMask = "LARGE OBJECT IGNORED")
        {
            var settings = new JsonSerializerSettings();

            if (retain != null || ignore != null || turns != null || ignoreLargeObject)
            {
                // 设置序列化解析器
                settings.ContractResolver = new CustomContractResolver(retain, ignore, turns);
            }


            // 设置序列化时间格式
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            settings.Converters.Add(timeConverter);

            var result = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);

            if (ignoreLargeObject)
            {
                var jObject = JToken.Parse(result);
                IgnoreLargeObject(jObject, largeObjectLengthLimit, largeObjectIgnoreMask);
                result = jObject.ToString();
            }

            return result;
        }

        /// <summary>
        /// 表格序列化
        /// </summary>
        /// <param name="rows">数据总行数</param>
        /// <param name="obj">对象</param>
        /// <param name="retain">保留属性</param>
        /// <param name="ignore">忽略属性</param>
        /// <param name="turns">要转换的属性</param>
        /// <returns>返回数据</returns>
        public static string Grid(object obj, int count,
            string[] retain = null,
            string[] ignore = null,
            IDictionary<string, string> turns = null)
        {
            return Serialize(new { total = count, rows = obj }, retain, ignore, turns);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="obj">数据</param>
        /// <returns>返回对象</returns>
        public static T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }


        /// <summary>
        /// 自定义序列化解析器
        /// </summary>
        public class CustomContractResolver : DefaultContractResolver
        {
            string[] retain = null;
            string[] ignore = null;
            IDictionary<string, string> turns = null;


            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="retain">保留属性</param>
            /// <param name="ignore">忽略属性</param>
            /// <param name="turns">要转换的属性</param>
            public CustomContractResolver(
                string[] retain = null,
                string[] ignore = null,
                IDictionary<string, string> turns = null
                )
            {
                // 指定要序列化属性的清单
                this.retain = retain;
                // 指定要忽略属性的清单
                this.ignore = ignore;
                // 指定要转换名称属性的清单
                this.turns = turns;
            }

            /// <summary>
            /// 重写属性创建函数
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <param name="memberSerialization">成员序列化对象</param>
            /// <returns>成员属性集合</returns>
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                List<JsonProperty> list = base.CreateProperties(type, memberSerialization).ToList();

                if (retain != null)
                {
                    // 只保留清单有列出的属性
                    list = list.Where(p => retain.Contains(p.PropertyName)).ToList();
                }

                if (ignore != null)
                {
                    // 只保留清单有列出的属性
                    list = list.Where(p => !ignore.Contains(p.PropertyName)).ToList();
                }

                if (turns != null)
                {
                    // 修改需要修改名称的属性
                    foreach (var turn in turns)
                    {
                        list.ForEach(new Action<JsonProperty>((m) =>
                        {
                            if (m.PropertyName == turn.Key)
                            {
                                m.PropertyName = turn.Value;
                            }
                        }));
                    }
                }

                return list;
            }
        }

        private static void IgnoreLargeObject(JToken jToken, int largeObjectLengthLimit, string largeObjectIgnoreMask)
        {
            // 如果是JObject，遍历其属性
            if (jToken is JObject jObject)
            {
                foreach (JProperty property in jObject.Properties().ToList())
                {
                    // 如果是字符串且长度超过限制，则移除该属性
                    if (property.Value.Type == JTokenType.String && ((string)property.Value).Length > largeObjectLengthLimit)
                    {
                        property.Value = largeObjectIgnoreMask;
                    }
                    else
                    {
                        // 否则，递归处理子对象或数组
                        IgnoreLargeObject(property.Value, largeObjectLengthLimit, largeObjectIgnoreMask);
                    }
                }
            }
            // 如果是JArray，遍历其元素
            else if (jToken is JArray jArray)
            {
                for (int i = 0; i < jArray.Count; i++)
                {
                    // 递归处理数组中的每个元素
                    IgnoreLargeObject(jArray[i], largeObjectLengthLimit, largeObjectIgnoreMask);
                }
            }
        }

        public static Dictionary<string, string> JsonToDictionary(string jsonStr)
        {
            Dictionary<string, string> dic = null;
            try
            {
                var jsonToken = JToken.Parse(jsonStr);
                switch (jsonToken.Type)
                {
                    case JTokenType.Array:
                        var jsonArray = JArray.Parse(jsonStr);
                        dic = new Dictionary<string, string>();
                        for (var i = 0; i < jsonArray.Count; i++)
                        {
                            var token = jsonArray[i].ToString();
                            var indexStr = $"ARRAY[{i}]";
                            dic.Add(indexStr, token);
                        }
                        break;
                    case JTokenType.Object:
                        var jsonObject = JObject.Parse(jsonStr);
                        dic = new Dictionary<string, string>();
                        foreach (var property in jsonObject.Properties())
                        {
                            dic.Add(property.Name, property.Value?.ToString());
                        }
                        break;

                }

            }
            catch
            {
            }
            return dic;
        }
    }
}
