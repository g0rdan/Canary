using System;
using Canary.Battery;
using NUnit.Framework;

namespace Battery.Droid.Tests
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void CheckChargingOfBattery()
        {
            var batteryProvider = new Canary.Battery.Droid.Battery();
            Assert.True(batteryProvider.IsCharging);
        }

        [Test]
        public void CheckBatteryLevel()
        {
            var batteryProvider = new Canary.Battery.Droid.Battery();
            Assert.True(batteryProvider.BatteryLevel >= 0 && batteryProvider.BatteryLevel <= 1);
        }

        [Test]
        public void CheckBatteryPowerType()
        {
            var batteryProvider = new Canary.Battery.Droid.Battery();
            // if we're running on a real device it should be plugged and Apple SDK does not
            // provide AC/USB thing. We can see only as AC.
            Assert.True(batteryProvider.PowerSource == PowerSourceType.AC);
        }

        [Test]
        public void CheckBatteryState()
        {
            var batteryProvider = new Canary.Battery.Droid.Battery();
            Assert.True(batteryProvider.BatteryState == ChargingState.Charging || batteryProvider.BatteryState == ChargingState.Full);
        }
    }
}
