using MediatR;
using QueryCQRS.Queries;
using Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QueryCQRS.Handlers
{
    public class BenchmarkHANDLER : IRequestHandler<BenchmarkREQUEST, BenchmarkRESPONSE>
    {
        private readonly BenchmarkCryptoTransf benchmark;
        public BenchmarkHANDLER(BenchmarkCryptoTransf benchmark)
        {
            this.benchmark = benchmark;
        }
        public async Task<BenchmarkRESPONSE> Handle(BenchmarkREQUEST request, CancellationToken cancellationToken)
        {
            benchmark.CountofData = request.CountOfData;
            benchmark.CountOfCycles = request.CountOfCycles;
            benchmark.CountOfKey = request.CountOfKey;

            List<IEnumerable<object[]>> result = new();

            if (request.FuncECBes != null && request.FuncECBes.Count != 0)
            {
                foreach (var el in request.FuncECBes)
                {
                    benchmark.DefaultStep = (int)el[0];
                    benchmark.FuncEBC = (Func<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>>)el[1];
                    result.Add(benchmark.BenchmarkRan(() => benchmark.ECB_Mode()));
                }
            }
            if(request.FuncCBCes != null && request.FuncCBCes.Count != 0)
            {
                foreach (var el in request.FuncCBCes)
                {
                    benchmark.DefaultStep = (int)el[0];
                    benchmark.FuncCBC = (Func<IEnumerable<byte>, IEnumerable<byte>, int, IEnumerable<byte>>)el[1];
                    result.Add(benchmark.BenchmarkRan(() => benchmark.CBC_Mode()));
                }
            }
            return await Task.Run(()=> new BenchmarkRESPONSE(result));
        }
    }
}
