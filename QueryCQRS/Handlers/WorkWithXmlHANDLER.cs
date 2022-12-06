using MediatR;
using QueryCQRS.Queries;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueryCQRS.Handlers
{
    public class SaveXmlSettingFileHANDLER : IRequestHandler<SaveXmlSettingFileREQUEST, SaveXmlSettingFileRESPONSE>
    {
        public async Task<SaveXmlSettingFileRESPONSE> Handle(SaveXmlSettingFileREQUEST request, CancellationToken cancellationToken)
        {
            try
            {
                var file = WorkWithXml.GetXmlFileFromDictionary(request.Pairs, request.PairsName);
                file.Save(request.Stream);
                return await Task.Run(() => new SaveXmlSettingFileRESPONSE());
            }
            catch
            {
                return await Task.Run(() => new SaveXmlSettingFileRESPONSE());
            }
        }
    }
    public class OpenXmlSettingFileHANDLER : IRequestHandler<OpenXmlREQUEST, OpenXmlRESPONSE>
    {
        public async Task<OpenXmlRESPONSE> Handle(OpenXmlREQUEST request, CancellationToken cancellationToken)
        {
            var pairs = WorkWithXml.GetDictionaryFromXmlFile(request.Strem, request.RootName);
            return await Task.Run(() => new OpenXmlRESPONSE(pairs));
        }
    }
}
