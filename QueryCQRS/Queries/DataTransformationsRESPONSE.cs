using System.Collections.Generic;

namespace QueryCQRS.Queries
{
    public class GetInputFromFileRESPONSE<T>
    {
        public T data { get; private set; }
        public GetInputFromFileRESPONSE(T data)
        {
            this.data = data;
        }
    }
    public class SaveDataInFileRESPONSE
    {
    }
    public class CryptoTransformRESPONSE<K>
    {
        public K CurrentDataState { get; private set; }
        public CryptoTransformRESPONSE(K currentDataState)
        {
            this.CurrentDataState = currentDataState;
        }
    }
}
