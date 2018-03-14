using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        public float CurrentFrequency => GetFrequency("/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq");

        public float MinFrequency => GetFrequency("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_min_freq");

        public float MaxFrequency => GetFrequency("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq");

        public int Cores => Runtime.GetRuntime().AvailableProcessors();

        public async Task<float> Usage (CancellationTokenSource cts = null)
        {
            if (cts != null)
            {
                return await Task.Factory.StartNew(GetSummaryUsage, cts.Token);
            }
            else
            {
                return await Task.Factory.StartNew(GetSummaryUsage);
            }

        }

        public async Task<IList<(string Key, string Value, string Description)>> AdditionalInformation(CancellationTokenSource cts = null)
        {
            if (cts != null)
            {
                return await Task.Factory.StartNew(GetStructedCPUInfo, cts.Token);
            }
            else
            {
                return await Task.Factory.StartNew(GetStructedCPUInfo);
            }
        }

        // https://stackoverflow.com/questions/16963292/read-current-cpu-frequency/19858957#19858957
        float GetFrequency(string path)
        {
            using (var reader = new RandomAccessFile(path, "r"))
            {
                float.TryParse(reader.ReadLine(), out var result);
                return result / 1000;
            }
        }

        #region Additional info
        IList<(string Key, string Value, string Description)> GetStructedCPUInfo()
        {
            var data = new List<(string Key, string Value, string Description)>();
            foreach (var item in CPUInfo)
            {
                data.Add((item.Key, item.Value, string.Empty));
            }
            return data;
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
        #endregion

        #region CPU Usage
        float GetSummaryUsage()
        {
            var statistics = GetCpuUsageStatistic();
            var user = statistics[0];
            var system = statistics[1];
            var idle = statistics[2];
            var other = statistics[3];
            return (user + system + idle + other) / 4f;
        }

        /// <summary>
        /// https://stackoverflow.com/a/10269661/1466039
        /// </summary>
        /// <returns>integer Array with 4 elements: user, system, idle and other cpu usage in percentage.</returns>
        int[] GetCpuUsageStatistic()
        {
            var tempString = ExecuteTop();
            if (string.IsNullOrWhiteSpace(tempString))
                return new int[4] { 0, 0, 0, 0 };

            tempString = tempString.Replace(",", "");
            tempString = tempString.Replace("User", "");
            tempString = tempString.Replace("System", "");
            tempString = tempString.Replace("IOW", "");
            tempString = tempString.Replace("IRQ", "");
            tempString = tempString.Replace("%", "");
            for (int i = 0; i < 10; i++)
            {
                tempString = tempString.Replace("  ", " ");
            }
            tempString = tempString.Trim();
            string[] myString = tempString.Split(' ');
            int[] cpuUsageAsInt = new int[myString.Length];
            for (int i = 0; i < myString.Length; i++)
            {
                myString[i] = myString[i].Trim();
                cpuUsageAsInt[i] = int.Parse(myString[i]);
            }
            return cpuUsageAsInt;
        }

        string ExecuteTop()
        {
            Process p = null;
            string returnString = null;
            try
            {
                p = Runtime.GetRuntime().Exec("top -n 1");
                using (var reader = new BufferedReader(new InputStreamReader(p.InputStream)))
                {
                    while (string.IsNullOrWhiteSpace(returnString))
                    {
                        returnString = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                System.Diagnostics.Debug.Write("error in getting first line of top", "executeTop");
            }
            finally
            {
                try
                {
                    p.Destroy();
                }
                catch (IOException)
                {
                    System.Diagnostics.Debug.Write("error in closing and destroying top process", "executeTop");
                }
            }
            return returnString;
        }
        #endregion
    }
}
