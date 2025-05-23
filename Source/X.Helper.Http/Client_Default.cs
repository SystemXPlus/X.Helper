using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public partial class Client
    {
        /// <summary>
        /// 将当前实例属性设置为默认值
        /// </summary>
        /// <returns></returns>
        [Obsolete("弃用", true)]
        public Client SetDefault()
        {

            return this;
        }
    }
}
