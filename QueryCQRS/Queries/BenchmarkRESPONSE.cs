using System.Collections.Generic;

namespace QueryCQRS.Queries
{
    public class BenchmarkRESPONSE
    {
        public IEnumerable<IEnumerable<object[]>> Data { get; private set; }
        public BenchmarkRESPONSE(IEnumerable<IEnumerable<object[]>> Data) 
        {
            this.Data = Data;
        }
    }
}
