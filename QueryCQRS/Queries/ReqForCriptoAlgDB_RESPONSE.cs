using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryCQRS.Queries
{
    public class ReqForCriptoAlgDB_RESPONSE
    {
        public IEnumerable<object[]> data { get; private set; }
        public ReqForCriptoAlgDB_RESPONSE(IEnumerable<object[]> data)
        {
            this.data = data;
        }
    }
}
