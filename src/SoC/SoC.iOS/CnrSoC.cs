using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;


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

        public Task<List<AdditionalInformation>> GetAdditionalInformationAsync(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<UsageInformation> GetUsageAsync(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }
    }
}
