using System;
using NUnit.Framework;
using Canary.Battery;

namespace Battery.iOS.Tests
{
    /// <summary>
    /// These tests should run only on a real devices
    /// </summary>
    [TestFixture]
    public class Tests
    {        
        [Test]
        public void CheckChargingOfBattery()
        {
            var batteryProvider = new Canary.Battery.iOS.CnrBattery();
            Assert.True(batteryProvider.IsCharging);
        }

        [Test]
        public void CheckBatteryLevel()
        {
            var batteryProvider = new Canary.Battery.iOS.CnrBattery();
            Assert.True(batteryProvider.BatteryLevel >= 0 && batteryProvider.BatteryLevel <= 1);
        }

        [Test]
        public void CheckBatteryPowerType()
        {
            var batteryProvider = new Canary.Battery.iOS.CnrBattery();
            // if we're running on a real device it should be plugged and Apple SDK does not
            // provide AC/USB thing. We can see only as AC.
            Assert.True(batteryProvider.PowerSource == PowerSourceType.AC);
        }

        [Test]
        public void CheckBatteryState()
        {
            var batteryProvider = new Canary.Battery.iOS.CnrBattery();
            Assert.True(batteryProvider.BatteryState == ChargingState.Charging || batteryProvider.BatteryState == ChargingState.Full);
        }
    }
}
