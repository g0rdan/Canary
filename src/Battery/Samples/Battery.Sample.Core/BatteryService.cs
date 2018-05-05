using System;
using System.Collections.Generic;
using Canary.Battery;
using Canary.Battery.Abstraction;

namespace Battery.Sample.Core
{
    public class BatteryService
    {
        public bool IsCharging { get { return CanaryBattery.Current.IsCharging; } }
        public float BatteryLevel { get { return (int)(CanaryBattery.Current.BatteryLevel * 100); } }
        public string BatteryState { get { return CanaryBattery.Current.BatteryState.ToString(); } }
        public string PowerType { get { return CanaryBattery.Current.PowerSource.ToString(); } }
        public IList<AdditionalInformation> AddInfo { get { return CanaryBattery.Current.AdditionalInformation; } }
    }
}
