using Canary.Battery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.Sample.Core
{
    public static class SomeService
    {
        public static int GetBatteryPercentage()
        {
            var percent = (int)(CanaryBattery.Current.BatteryLevel * 100);
            return percent;
        }
    }
}
