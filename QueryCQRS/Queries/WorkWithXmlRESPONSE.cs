using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QueryCQRS.Queries
{
    public class SaveXmlSettingFileRESPONSE
    {
    }
    public class OpenXmlRESPONSE
    {
        public Dictionary<string, object> Pairs { get; private set; }
        public OpenXmlRESPONSE(Dictionary<string, object> pairs)
        {
            this.Pairs = pairs;
        }
    }
}
