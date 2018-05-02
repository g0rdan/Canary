using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Canary.SoC
{
    public interface ICnrSoC
    {
        /// <summary>
        /// Name of chip
        /// </summary>
        string Model { get; }
        /// <summary>
        /// Current frequency in MHz
        /// </summary>
        float CurrentFrequency { get; }
        float MinFrequency { get; }
        float MaxFrequency { get; }
        /// <summary>
        /// Amount of cores in SoC
        /// </summary>
        int Cores { get; }
        /// <summary>
        /// Current usage of central processor (from 0 to 1 in float) 
        /// </summary>
        Task<UsageInformation> GetUsageAsync(CancellationToken token = default(CancellationToken));
        /// <summary>
        /// This property needs to show some additional
        /// information about SoC on a device, which could not be
        /// represent as common information for many platforms.
        /// </summary>
        Task<List<AdditionalInformation>> GetAdditionalInformationAsync(CancellationToken token = default(CancellationToken));
    }
}
