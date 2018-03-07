using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Battery
{
    interface IBattery
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
    }

    public enum ChargingState
    {
        Unknown,
        Unplugged,
        Charging,
        Full
    }
}
