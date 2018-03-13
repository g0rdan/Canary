using System;
using System.Collections.Generic;

namespace SoC.Core
{
    public interface ICnrSoC
    {
        /// <summary>
        /// Name of chip
        /// </summary>
        string Model { get; }
        /// <summary>
        /// Current usage of central processor (from 0 to 1 in float) 
        /// </summary>
        float Usage { get; }
        /// <summary>
        /// Current frequency in MHz
        /// </summary>
        float Frequency { get; }
        /// <summary>
        /// Amount of cores in SoC
        /// </summary>
        int Cores { get; }
        /// <summary>
        /// This property needs to show some additional
        /// information about SoC on a device, which could not be
        /// represent as common information for many platforms.
        /// </summary>
        IList<(string Key, string Value, string Description)> AdditionalInformation { get; }
    }
}
