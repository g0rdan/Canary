using Canary.Battery.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Power;

namespace Canary.Battery
{
    public class CnrBattery : ICnrBattery
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

        public IList<AdditionalInformation> AdditionalInformation
        {
            get
            {
                var report = GetAggregateBatteriesReport();
                var data = new List<AdditionalInformation>();
                if (report.ChargeRateInMilliwatts.HasValue)
                {
                    data.Add(new AdditionalInformation(nameof(report.ChargeRateInMilliwatts), report.ChargeRateInMilliwatts.ToString(), "The rate that the battery is charging in milliwatts (mW). This value is negative when the battery is discharging."));
                }
                if (report.DesignCapacityInMilliwattHours.HasValue)
                {
                    data.Add(new AdditionalInformation(nameof(report.DesignCapacityInMilliwattHours), report.DesignCapacityInMilliwattHours.ToString(), "The estimated energy capacity of a new battery of this type, in milliwatt-hours (mWh)."));
                }
                if (report.FullChargeCapacityInMilliwattHours.HasValue)
                {
                    data.Add(new AdditionalInformation(nameof(report.FullChargeCapacityInMilliwattHours), report.FullChargeCapacityInMilliwattHours.ToString(), "The fully-charged energy capacity of the battery, in milliwatt-hours (mWh). Note: Some devices report their battery capacity in milliamp-hours (mAh) instead of mWh. As a rough heuristic, if the value reported is lower than 4400 it is likely represented in mAh, otherwise it is in mWh."));
                }
                if (report.RemainingCapacityInMilliwattHours.HasValue)
                {
                    data.Add(new AdditionalInformation(nameof(report.RemainingCapacityInMilliwattHours), report.RemainingCapacityInMilliwattHours.ToString(), "The remaining power capacity of the battery, in milliwatt-hours."));
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
