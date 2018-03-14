using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Canary.SoC.iOS
{
    public class CnrSoC : ICnrSoC
    {
        public CnrSoC()
        {
        }

        public string Model => throw new NotImplementedException();

        public float CurrentFrequency => throw new NotImplementedException();

        public float MinFrequency => throw new NotImplementedException();

        public float MaxFrequency => throw new NotImplementedException();

        public int Cores => throw new NotImplementedException();

        public Task<IList<(string Key, string Value, string Description)>> AdditionalInformation(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<float> Usage(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }
    }
}
