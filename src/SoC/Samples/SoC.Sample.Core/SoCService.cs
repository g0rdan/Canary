using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Canary.SoC;

namespace SoC.Sample.Core
{
    public class SoCService
    {
        public string Model { get { return CanarySoC.Current.Model; } }
        public float MaxFrequency { get { return CanarySoC.Current.MaxFrequency; } }
        public float MinFrequency { get { return CanarySoC.Current.MinFrequency; } }
        public int Cores { get { return CanarySoC.Current.Cores; } }

        public async Task<Dictionary<string, string>> GetAddInfo()
        {
            var dict = new Dictionary<string, string>();
            var data = await CanarySoC.Current.GetAdditionalInformationAsync();
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    dict.Add(item.Title, item.Value);
                }
            }
            return dict;
        }
    }
}
