using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Services
{
    public class BenchmarkCryptoTransf
    {
        public Func<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>>? FuncEBC { get; set; }
        public Func<IEnumerable<byte>, IEnumerable<byte>, int, IEnumerable<byte>>? FuncCBC { get; set; }
        public int CountofData { get; set; }
        public int CountOfCycles { get; set; }
        public int CountOfKey { get; set; }
        public int DefaultStep { get; set; }

        private List<byte> data;
        private List<byte> key;
        private int initVec;
        private Random random;
        private readonly int startRandVec;
        private readonly Stopwatch stopwatch;

        public BenchmarkCryptoTransf()
        {
            this.data = new List<byte>();
            this.key = new List<byte>();
            this.startRandVec = DateTime.Now.Second;
            this.stopwatch = new();
        }
        public void ECB_Mode() => FuncEBC(data, key);

        public void CBC_Mode() => FuncCBC(data, key, initVec);

        private void FormingStartData(double count)
        {
            this.key.Clear();
            while(this.data.Count < count) { this.data.Add((byte)this.random.Next(0, 255)); }
            while(this.key.Count < this.CountOfKey) { this.key.Add((byte)this.random.Next(0, 255)); }
            initVec = this.random.Next(0, 1000);
        }
        public IEnumerable<object[]> BenchmarkRan(Action action)
        {
            this.random = new(this.startRandVec);
            data.Clear();
            key.Clear();
            List<object[]> result = new();
            double step = (double)CountofData / (double)CountOfCycles;

            for (int i = 0; i <= this.CountOfCycles;)
            {      
                try
                {
                    stopwatch.Restart();
                    action.Invoke();
                    stopwatch.Stop();
                    i++;
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Не корректные входные данные.")
                    {
                        this.FormingStartData(this.DefaultStep);
                        step = this.DefaultStep;
                        result.Clear();
                        continue;
                    }
                }
                result.Add(new object[2] { this.data.Count, stopwatch.Elapsed.TotalMilliseconds });
                if (this.data.Count + step < this.CountofData) { this.FormingStartData(this.data.Count + step); }
                else { return result; }
            }
            return result;
        }
    }
}


