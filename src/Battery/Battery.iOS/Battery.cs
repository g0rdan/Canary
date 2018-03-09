using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Canary.Battery.iOS
{
    public class CnrBattery : ICnrBattery
    {
        public bool IsCharging => UIDevice.CurrentDevice.BatteryState == UIDeviceBatteryState.Charging;

        public float BatteryLevel => UIDevice.CurrentDevice.BatteryLevel;

        public ChargingState BatteryState
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Unknown:
                        return ChargingState.Unknown;
                    case UIDeviceBatteryState.Unplugged:
                        return ChargingState.Discharging;
                    case UIDeviceBatteryState.Charging:
                        return ChargingState.Charging;
                    case UIDeviceBatteryState.Full:
                        return ChargingState.Full;
                    default:
                        return ChargingState.Unknown;
                }
            }
        }
        
        public PowerSourceType PowerSource
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Unknown:
                        return PowerSourceType.Unknown;
                    case UIDeviceBatteryState.Unplugged:
                        return PowerSourceType.Battery;
                    case UIDeviceBatteryState.Charging:
                        return PowerSourceType.AC;
                    case UIDeviceBatteryState.Full:
                        return PowerSourceType.AC;
                    default:
                        return PowerSourceType.Unknown;
                }
            }
        }

        public IList<(string Key, string Value, string Description)> AdditionalInformation => 
            throw new NotImplementedException("iOS does not have additional information about battery");
    }
}