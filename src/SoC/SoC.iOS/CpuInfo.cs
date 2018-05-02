using System;
namespace Canary.SoC
{
    internal class CpuInfo
    {
        /// <summary>
        /// CPU click in Mhz
        /// In this case it will be max cpu speed
        /// </summary>
        public string CPUClock { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// In Kb
        /// </summary>
        public string L1Cache { get; set; }
        /// <summary>
        /// In Kb
        /// </summary>
        public string L2Cache { get; set; }
        /// <summary>
        /// In Kb
        /// </summary>
        public string L3Cache { get; set; }
        /// <summary>
        /// 32 or 64 bits
        /// </summary>
        public string Capacity { get; set; }
        public string Architecture { get; set; }

        public CpuInfo()
        {
        }
    }
}
