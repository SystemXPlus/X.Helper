using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;

namespace X.Helper.Http.Helper.HttpContentCreator
{
    public class ApplicationOctetStreamHttpContentCreator : Abstract.HttpContentCreatorBase
    {

        public override HttpContent Create(IEnumerable<HttpContentParam> contentParams, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
