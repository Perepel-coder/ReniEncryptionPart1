using Cipher.Cipher_Algorithms;
using MediatR;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace QueryCQRS.Queries
{
    public enum Mode { ECB, CBC, GAMMING_ECB }
    public enum Orientation { ENCODE, DECODE }
    public enum Alg { AES, GOST89 }
    public class GetDataFromFileREQUEST<T> : IRequest<GetInputFromFileRESPONSE<T>>
    {
        public FileStream Stream { get; private set; }
        public GetDataFromFileREQUEST(FileStream stream) 
        {
            this.Stream = stream;
        }
    }
    public class SaveDataInFileREQUEST<T, K>: IRequest<SaveDataInFileRESPONSE>
    {
        public K Data { get; private set; }
        public FileStream Stream { get; private set; }
        public SaveDataInFileREQUEST(K data, FileStream stream)
        {
            this.Data = data;
            this.Stream = stream;
        }
    }
    public class CryptoTransformREQUEST<T, K> : IRequest<CryptoTransformRESPONSE<K>>
    {
        private Action? function;
        private IEnumerable<byte>? result;
        private List<byte>? inputData;

        public Orientation Orientation { get; private set; }

        public CryptoTransformREQUEST(
            T data, 
            char complementarySymbol, 
            string key, 
            Mode mode, 
            Alg alg, 
            Orientation orientation, 
            int initVec)
        {
            if (typeof(T) == typeof(string) && data != null)
            {
                string str = (string)Convert.ChangeType(data, typeof(string));
                this.inputData = DataTransformations.GetToByte(str).ToList();
            }
            else if (typeof(T) == typeof(DataTable) && data != null)
            {
                DataTable table = (DataTable)Convert.ChangeType(data, typeof(DataTable));
                this.inputData = DataTransformations.GetToByte(table).ToList();
            }
            else { throw new Exception("Неизвестный тип данных."); }
            this.Orientation = orientation;
            IEnumerable<byte> keyNum = DataTransformations.GetToByte(key);
            ICipherBlockAlgorithm method = GetMethod(alg, complementarySymbol);
            if (orientation == Orientation.ENCODE)
            {
                if (mode == Mode.ECB) { function = ()=> result = method.EncodeECB(this.inputData, keyNum); } 
                if (mode == Mode.CBC) { function = ()=> result = method.EncodeCBC(this.inputData, keyNum, initVec); }
                if (mode == Mode.GAMMING_ECB) { function = ()=> result = method.EncodeGammingECB(this.inputData, keyNum, initVec); }
            }
            if (orientation == Orientation.DECODE)
            {
                if (mode == Mode.ECB) { function = ()=> result = method.DecodeECB(this.inputData, keyNum); }
                if (mode == Mode.CBC) { function = ()=> result = method.DecodeCBC(this.inputData, keyNum, initVec); }
                if (mode == Mode.GAMMING_ECB) { function = ()=> result = method.DecodeGammingECB(this.inputData, keyNum, initVec); }
            }
        }
        public IEnumerable<byte> Run()
        {
            if (this.function != null) 
            { 
                function.Invoke();
                return result != null ? result : new List<byte>();
            }
            throw new Exception("Ошибка инициализации процесса");
        }
        private ICipherBlockAlgorithm GetMethod(Alg alg, char symbol)
        {
            if (alg == Alg.AES) { return new Aes(4, 4, symbol); }
            if (alg == Alg.GOST89) { return new GOST89(symbol); }
            throw new Exception("Попытка получить неизвестный параметр");
        }
    }
}