using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Java.IO;
using Java.Lang;
using Java.Util;

namespace Canary.SoC
{
    public class CnrSoC : ICnrSoC
    {
        const string MODEL_NAME_KEY = "model name";
        const string CURR_FREQUENCY_PATH = "/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq";
        const string MIN_FREQUENCY_PATH = "/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_min_freq";
        const string MAX_FREQUENCY_PATH = "/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq";
        const string CPU_INFO_PATH = "/proc/cpuinfo";
        const string TOP_COMMAND = "top -n 1";

        public string Model
        {
            get
            {
                if (CPUInfo.ContainsKey(MODEL_NAME_KEY))
                    return CPUInfo[MODEL_NAME_KEY];
                return string.Empty;
            }
        }

        public float CurrentFrequency => GetFrequency(CURR_FREQUENCY_PATH);

        public float MinFrequency => GetFrequency(MIN_FREQUENCY_PATH);

        public float MaxFrequency => GetFrequency(MAX_FREQUENCY_PATH);

        public int Cores => Runtime.GetRuntime().AvailableProcessors();

        public async Task<UsageInformation> GetUsageAsync (CancellationToken token = default(CancellationToken))
        {
            return await Task.Factory.StartNew(GetSummaryUsage, token).ConfigureAwait(false);
        }

        public async Task<List<AdditionalInformation>> GetAdditionalInformationAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Factory.StartNew(GetStructedCPUInfo, token).ConfigureAwait(false);
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
        List<AdditionalInformation> GetStructedCPUInfo()
        {
            var data = new List<AdditionalInformation>();
            foreach (var item in CPUInfo)
            {
                data.Add(new AdditionalInformation {
                    Title = item.Key,
                    Value = item.Value,
                    Description = string.Empty
                });
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
                    using (var s = new Scanner(new File(CPU_INFO_PATH)))
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
        UsageInformation GetSummaryUsage()
        {
            var statistics = GetCpuUsageStatistic();
            return new UsageInformation
            {
                User = statistics[0],
                System = statistics[1],
                Idle = statistics[2],
                Other = statistics[3]
            };
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
                p = Runtime.GetRuntime().Exec(TOP_COMMAND);
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
