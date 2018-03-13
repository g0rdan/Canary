using System;
using System.Collections.Generic;
using Java.IO;
using Java.Lang;
using Java.Util;

namespace Canary.SoC.Droid
{
    public class CnrSoC : ICnrSoC
    {
        public CnrSoC()
        {
        }

        public string Model => CPUInfo["model name"];

        public float Usage => throw new NotImplementedException();

        public float Frequency => throw new NotImplementedException();

        public int Cores => Runtime.GetRuntime().AvailableProcessors();

        public IList<(string Key, string Value, string Description)> AdditionalInformation
        {
            get
            {
                var data = new List<(string Key, string Value, string Description)>();
                foreach (var item in CPUInfo)
                {
                    data.Add((item.Key, item.Value, string.Empty));
                }
                return data;
            }
        }

        Dictionary<string, string> _cPUInfo;
        Dictionary<string, string> CPUInfo
        {
            get
            {
                if (_cPUInfo != null)
                    return _cPUInfo;

                var info = new Dictionary<string, string>();
                try
                {
                    using (var s = new Scanner(new File("/proc/cpuinfo")))
                    {
                        while (s.HasNextLine)
                        {
                            var vals = s.NextLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            if (vals.Length > 1)
                                info.Add(vals[0].Trim(), vals[1].Trim());
                        }    
                    }
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"An exception in ICnrSoC: {ex.Message}");
                }

                _cPUInfo = info;
                return _cPUInfo;
            }
        }
    }
}
