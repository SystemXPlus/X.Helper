using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Entity
{
    public class CustomRequestHeader
    {
        public CustomRequestHeader(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException("name is null");
            Value = value??string.Empty;
        }

        public CustomRequestHeader(string name, IEnumerable<string> value)
        {
            Name = name ?? throw new ArgumentNullException("name is null");
            Value = value ?? throw new ArgumentNullException("value is null");
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public bool HasAdded { get; set; } = false;
    }
}
