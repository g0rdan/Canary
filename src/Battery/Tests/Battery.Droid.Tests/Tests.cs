using System;
using Canary.Battery;
using NUnit.Framework;

namespace Battery.Droid.Tests
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
            Assert.True(batteryProvider.PowerSource == PowerSourceType.USB);
        }

        [Test]
        public void CheckBatteryState()
        {
            var batteryProvider = new Canary.Battery.Droid.Battery();
            Assert.True(batteryProvider.BatteryState == ChargingState.Charging || batteryProvider.BatteryState == ChargingState.Full);
        }
    }
}
