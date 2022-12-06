using MediatR;
using QueryCQRS.Queries;
using Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QueryCQRS.Handlers
{
    public class ReqForCriptoAlgDB_HANDLER : IRequestHandler<ReqForCriptoAlgDB_REQUEST, ReqForCriptoAlgDB_RESPONSE>
    {
        private readonly ReqForCriptoAlgDB algDB;
        public ReqForCriptoAlgDB_HANDLER(ReqForCriptoAlgDB algDB) 
        { 
            this.algDB = algDB; 
        }
        public async Task<ReqForCriptoAlgDB_RESPONSE> Handle(ReqForCriptoAlgDB_REQUEST request, CancellationToken cancellationToken)
        {
            if(request.QurrentMode == ReqForCriptoAlgDB_REQUEST.Mode.GET_LIST_OF_ALG)
            {
                return new ReqForCriptoAlgDB_RESPONSE(await Task.Run(() => algDB.GetListOfCryptoAlgorithms()));
            }
            if (request.QurrentMode == ReqForCriptoAlgDB_REQUEST.Mode.GET_MODE_OF_ALG)
            {
                if(request.Data == null) { throw new Exception("Пустые входные данные RequestsForCriptoAlgDB_HANDLER"); }
                return new ReqForCriptoAlgDB_RESPONSE(
                    await Task.Run
                    (
                        () => algDB.GetListOfCryptoAlgorithmsMode( request.Data.Select(el => (int)el[1]))
                    ));
            }
            if(request.QurrentMode == ReqForCriptoAlgDB_REQUEST.Mode.GET_MODE_FOR_ID)
            {
                if (request.Data == null) { throw new Exception("Пустые входные данные RequestsForCriptoAlgDB_HANDLER"); }
                return new ReqForCriptoAlgDB_RESPONSE(
                    await Task.Run
                    (
                        () => algDB.GetModeForId( request.Data.Select(el=>(int)el[1]).Single())
                    ));
            }
            throw new Exception("Некорректный запрос");
        }
    }
}
