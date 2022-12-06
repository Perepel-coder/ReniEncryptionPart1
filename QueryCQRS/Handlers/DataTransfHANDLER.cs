using MediatR;
using QueryCQRS.Queries;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace QueryCQRS.Handlers
{
    public class GetDataFromFileHANDLER<T> : IRequestHandler<GetDataFromFileREQUEST<T>, GetInputFromFileRESPONSE<T>>
    {
        private readonly DataTransformations transform;
        public GetDataFromFileHANDLER(DataTransformations transform)
        {
            this.transform = transform;
        }
        public async Task<GetInputFromFileRESPONSE<T>> Handle(GetDataFromFileREQUEST<T> request, CancellationToken cancellationToken)
        {
            this.transform.Stream = request.Stream;
            if (typeof(T) == typeof(string)) 
            {
                var res = await Task.Run(() => this.transform.OpenStringFile());
                return new GetInputFromFileRESPONSE<T>((T)Convert.ChangeType(res, typeof(T)));
            }
            if (typeof(T) == typeof(DataTable))
            {
                var res = await Task.Run(() => this.transform.OpenDataTableFile());
                return new GetInputFromFileRESPONSE<T>((T)Convert.ChangeType(res, typeof(T)));
            }
            throw new Exception("Неизвестный тип данных");
        }
    }

    public class SaveDataInFileHANDLER<T, K> : IRequestHandler<SaveDataInFileREQUEST<T, K>, SaveDataInFileRESPONSE>
    {
        private readonly DataTransformations transform;
        public SaveDataInFileHANDLER(DataTransformations transform)
        {
            this.transform = transform;
        }
        public async Task<SaveDataInFileRESPONSE> Handle(SaveDataInFileREQUEST<T, K> request, CancellationToken cancellationToken)
        {
            if(typeof(T) == typeof(DataTable) && typeof(K) == typeof(string))
            {
                IEnumerable<byte> data = DataTransformations.GetToByte(
                    (string)Convert.ChangeType(request.Data, typeof(string)));
                transform.Stream = request.Stream;
                transform.SaveFileCTF(data);
                return await Task.Run(()=> new SaveDataInFileRESPONSE());
            }
            throw new Exception("Неизвестный тип данных");
        }
    }

    public class CryptoTransformHANDLER<T,K> : IRequestHandler<CryptoTransformREQUEST<T,K>, CryptoTransformRESPONSE<K>>
    {
        public async Task<CryptoTransformRESPONSE<K>> Handle(CryptoTransformREQUEST<T,K> request, CancellationToken cancellationToken)
        {
            IEnumerable<byte> result = request.Run();
            if (typeof(K) == typeof(string))
            {
                return await Task.Run(() => new CryptoTransformRESPONSE<K>(
                    (K)Convert.ChangeType(
                        DataTransformations.GetString(result), typeof(K))));
            }
            if (typeof(K) == typeof(DataTable))
            {
                return await Task.Run(() => new CryptoTransformRESPONSE<K>(
                    (K)Convert.ChangeType(
                        DataTransformations.GetDataTable(result), typeof(K))));
            }
            throw new Exception("Неизвестный тип данных");
        }
    }
}
