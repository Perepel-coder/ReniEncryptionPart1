using MediatR;
using System.Collections.Generic;

namespace QueryCQRS.Queries
{
    public class ReqForCriptoAlgDB_REQUEST : IRequest<ReqForCriptoAlgDB_RESPONSE>
    {
        public enum Mode { GET_LIST_OF_ALG, GET_MODE_OF_ALG, GET_MODE_FOR_ID };
        public Mode QurrentMode { get; private set; }
        public IEnumerable<object[]>? Data { get; private set; }
        public ReqForCriptoAlgDB_REQUEST(Mode mode) { this.QurrentMode = mode; }
        public ReqForCriptoAlgDB_REQUEST(Mode mode, IEnumerable<object[]> data)
        {
            this.Data = data;
            this.QurrentMode = mode;
        }
    }
}
