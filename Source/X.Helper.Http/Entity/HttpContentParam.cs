using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Enums;

namespace X.Helper.Http.Entity
{
    public class HttpContentParam

    {


        public string Name { get; set; }
        public object Value { get; set; }

        public HttpContentParamType Type { get; set; }


        private void SetType()
        {
            if (Value == null)
            {
                Type = HttpContentParamType.Null;
                return;
            }
            switch (Value)
            {
                //case ValueType _ when Value.GetType().IsValueType:
                //    // 处理值类型
                //    Type = HttpContentParamType.Number;
                //    break;
                case string _:
                    Type = HttpContentParamType.String;
                    break;
                case int _:
                case short _:
                case decimal _:
                case float _:
                case long _:
                case double _:
                    Type = HttpContentParamType.Number;
                    break;
                case bool _:
                    Type = HttpContentParamType.Boolean;
                    break;
                case byte[] _:
                    Type = HttpContentParamType.Binary;
                    break;
                case System.IO.Stream _:
                    Type = HttpContentParamType.Stream;
                    break;
                case System.IO.FileInfo _:
                    Type = HttpContentParamType.File;
                    break;
                default:
                    Type = HttpContentParamType.Object;
                    break;
            }
        }

    }
}
