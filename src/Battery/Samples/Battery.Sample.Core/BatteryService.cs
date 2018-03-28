using System;
using System.Collections.Generic;
using System.Text;
using Canary.Battery;

namespace Battery.Sample.Core
{
    public static class BatteryService
    {
        public static int GetBatteryPercentage()
        {
            var percent = (int)(CanaryBattery.Current.BatteryLevel * 100);
            return percent;
        }
    }
}
