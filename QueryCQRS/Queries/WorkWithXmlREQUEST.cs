using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryCQRS.Queries
{
    public class SaveXmlSettingFileREQUEST: IRequest<SaveXmlSettingFileRESPONSE>
    {
        public Dictionary<string, object> Pairs { get; private set; }
        public string PairsName { get; private set; }
        public Stream Stream { get; private set; }
        public SaveXmlSettingFileREQUEST(
            string ComplementarySymbol,
            string StartKeyValue,
            int InitVec,
            Mode AlgModeSelectedItem,
            Alg AlgSelectedItem,
            Orientation OrientationSTR,
            Stream stream)
        {
            this.Pairs = new();
            this.Pairs.Add(nameof(ComplementarySymbol), ComplementarySymbol);
            this.Pairs.Add(nameof(StartKeyValue), StartKeyValue);
            this.Pairs.Add(nameof(InitVec), InitVec);
            this.Pairs.Add(nameof(AlgModeSelectedItem), AlgModeSelectedItem);
            this.Pairs.Add(nameof(AlgSelectedItem), AlgSelectedItem);
            this.Pairs.Add(nameof(OrientationSTR), OrientationSTR);
            this.PairsName = "settings";
            this.Stream = stream;
        }
    }
    public class OpenXmlREQUEST:IRequest<OpenXmlRESPONSE>
    {
        public Stream Strem { get; private set; }
        public string RootName { get; private set; }
        public OpenXmlREQUEST(Stream stream, string rootName)
        {
            this.Strem = stream;
            this.RootName = rootName;
        }
    }
}
