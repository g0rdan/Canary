using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Power;

namespace Canary.Battery.UWP
{
    public class Battery : IBattery
    {
        public bool IsCharging => GetAggregateBatteriesReport().Status == Windows.System.Power.BatteryStatus.Charging;

        public float BatteryLevel
        {
            get
            {
                var report = GetAggregateBatteriesReport();
                if (report.RemainingCapacityInMilliwattHours.HasValue && report.FullChargeCapacityInMilliwattHours.HasValue)
                {
                    return report.RemainingCapacityInMilliwattHours.Value / (float)report.FullChargeCapacityInMilliwattHours.Value;
                }

                return -1f;
            }
        }

        public ChargingState BatteryState
        {
            get
            {
                if (BatteryLevel >= 1.0f)
                {
                    return ChargingState.Full;
                }

                switch (GetAggregateBatteriesReport().Status)
                {
                    case Windows.System.Power.BatteryStatus.NotPresent:
                    case Windows.System.Power.BatteryStatus.Idle:
                        return ChargingState.Unknown;
                    case Windows.System.Power.BatteryStatus.Discharging:
                        return ChargingState.Discharging;
                    case Windows.System.Power.BatteryStatus.Charging:
                        return ChargingState.Charging;
                    default:
                        return ChargingState.Unknown;
                }
            }
        }

        public PowerSourceType PowerSource
        {
            get
            {
                var report = GetAggregateBatteriesReport();
                if (report.Status == Windows.System.Power.BatteryStatus.Charging || BatteryLevel >= 1.0f)
                {
                    return PowerSourceType.AC;
                }
                return PowerSourceType.Battery;
            }
        }

        public IDictionary<string, string> AdditionalInformation
        {
            get
            {
                var report = GetAggregateBatteriesReport();
                var data = new Dictionary<string, string>();
                if (report.ChargeRateInMilliwatts.HasValue)
                {
                    data.Add(nameof(report.ChargeRateInMilliwatts), report.ChargeRateInMilliwatts.ToString());
                }
                if (report.DesignCapacityInMilliwattHours.HasValue)
                {
                    data.Add(nameof(report.DesignCapacityInMilliwattHours), report.DesignCapacityInMilliwattHours.ToString());
                }
                if (report.FullChargeCapacityInMilliwattHours.HasValue)
                {
                    data.Add(nameof(report.FullChargeCapacityInMilliwattHours), report.FullChargeCapacityInMilliwattHours.ToString());
                }
                if (report.RemainingCapacityInMilliwattHours.HasValue)
                {
                    data.Add(nameof(report.RemainingCapacityInMilliwattHours), report.RemainingCapacityInMilliwattHours.ToString());
                }
                return data;
            }
        }

        // TODO Consider ability to use multiple batteries into plugin
        BatteryReport GetAggregateBatteriesReport()
        {
            // Create aggregate battery object
            var aggBattery = Windows.Devices.Power.Battery.AggregateBattery;
        
            // Get report
            return aggBattery.GetReport();
        }
    }
}
