using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Battery
{
    public interface IBattery
    {
        /// <summary>
        /// Shows state of charging. True if it's plugged to AC/USB
        /// </summary>
        bool IsCharging { get; }

        /// <summary>
        /// Shows battery level from 0.0 to 1.0 where 1.0 it's 100% charged.
        /// </summary>
        float BatteryLevel { get; }

        /// <summary>
        /// Gets charging state of battery
        /// </summary>
        ChargingState BatteryState { get; }

        /// <summary>
        /// Gets power source: AC, USB or wireless
        /// </summary>
        PowerSourceType PowerSource { get; }

        /// <summary>
        /// This property needs to show some additional
        /// information about battery on a device, which could not be
        /// represent as common information for many platforms.
        /// </summary>
        /// <value>The additional information.</value>
        IList<(string Key, string Value, string Description)> AdditionalInformation { get; }
    }

    public enum ChargingState
    {
        Unknown,
        Discharging,
        Charging,
        Full
    }

    public enum PowerSourceType
    {
        Unknown,
        Battery,
        USB,
        AC,
        Wireless
    }
}
