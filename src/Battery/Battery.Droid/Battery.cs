using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Canary.Battery.Droid
{
    public class Battery : IBattery
    {
        string exceptionMessage = $"You need to add '{Android.Manifest.Permission.BatteryStats}' to AndroidManifest.xml";

        public bool IsCharging => BatteryState == ChargingState.Charging || BatteryState == ChargingState.Full;

        public float BatteryLevel
        {
            get
            {
                if (!CheckBatteryPermissions())
                    throw new CanaryException(exceptionMessage);

                using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                {
                    using (var battery = Application.Context.RegisterReceiver(null, filter))
                    {
                        var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                        var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);
                        return level / (float)scale;
                    }
                }
            }
        }

        public ChargingState BatteryState
        {
            get
            {
                if (!CheckBatteryPermissions())
                    throw new CanaryException(exceptionMessage);

                using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                {
                    using (var battery = Application.Context.RegisterReceiver(null, filter))
                    {
                        var status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                        switch (status)
                        {
                            case (int)BatteryStatus.Charging:
                                return ChargingState.Charging;
                            case (int)BatteryStatus.Discharging:
                            case (int)BatteryStatus.NotCharging:
                                return ChargingState.Discharging;
                            case (int)BatteryStatus.Full:
                                return ChargingState.Full;
                            default:
                                return ChargingState.Unknown;
                        }
                    }
                }
            }
        }

        public PowerSourceType PowerSource
        {
            get
            {
                if (!CheckBatteryPermissions())
                    throw new CanaryException(exceptionMessage);

                if (!IsCharging)
                    return PowerSourceType.Battery;

                using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                {
                    using (var battery = Application.Context.RegisterReceiver(null, filter))
                    {
                        var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                        
                        switch (chargePlug)
                        {
                            case (int)BatteryPlugged.Usb:
                                return PowerSourceType.USB;
                            case (int)BatteryPlugged.Ac:
                                return PowerSourceType.AC;
                            case (int)BatteryPlugged.Wireless:
                                return PowerSourceType.Wireless;
                            default:
                                return PowerSourceType.Unknown;
                        }
                    }
                }
            }
        }

        bool CheckBatteryPermissions()
        {
            var permission = Android.Manifest.Permission.BatteryStats;
            var res = Application.Context.CheckCallingOrSelfPermission(permission);
            return res == Permission.Granted;
        }
    }
}