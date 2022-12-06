using Cipher.Cipher_Algorithms;
using MediatR;
using System;
using System.Collections.Generic;

namespace QueryCQRS.Queries
{
    public class BenchmarkREQUEST: IRequest<BenchmarkRESPONSE>
    {
        public List<object[]> FuncECBes { get; private set; }
        public List<object[]> FuncCBCes { get; private set; }
        public int CountOfData { get; private set; }
        public int CountOfCycles { get; private set; }
        public int CountOfKey { get; private set; }
        private int defaultStep;
        public BenchmarkREQUEST(IEnumerable<Alg> algs, Mode mode, Orientation orientation, int CountOfData, int CountOfCycles, int CountOfKey)
        {
            this.FuncECBes = new();
            this.FuncCBCes = new();
            this.CountOfCycles = CountOfCycles;
            this.CountOfData = CountOfData;
            this.CountOfKey = CountOfKey;
            foreach (var al in algs)
            {
                ICipherBlockAlgorithm method = GetMethod(al);
                this.defaultStep = method.BlockSize;

                if(orientation == Orientation.ENCODE)
                {
                    if (mode == Mode.ECB) { this.FuncECBes.Add(new object[2] { this.defaultStep, method.EncodeECB });}
                    if(mode == Mode.CBC) { this.FuncCBCes.Add(new object[2] { this.defaultStep, method.EncodeCBC }); }
                    if(mode == Mode.GAMMING_ECB) { this.FuncCBCes.Add(new object[2] { this.defaultStep, method.EncodeGammingECB }); }
                }
                if(orientation == Orientation.DECODE)
                {
                    if (mode == Mode.ECB) { this.FuncECBes.Add(new object[2] { this.defaultStep, method.DecodeECB }); }
                    if (mode == Mode.CBC) { this.FuncCBCes.Add(new object[2] { this.defaultStep, method.DecodeCBC }); }
                    if (mode == Mode.GAMMING_ECB) { this.FuncCBCes.Add(new object[2] { this.defaultStep, method.DecodeGammingECB }); }
                }
            } 
        }
        private ICipherBlockAlgorithm GetMethod(Alg alg)
        {
            if (alg == Alg.AES) { return new Aes(4, 4, '~'); }
            if (alg == Alg.GOST89) { return new GOST89('~'); }
            throw new Exception("Попытка получить неизвестный параметр");
        }
    }
}
