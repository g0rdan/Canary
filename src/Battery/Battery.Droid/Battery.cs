﻿using System;
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
                    using (var intent = Application.Context.RegisterReceiver(null, filter))
                    {
                        var level = intent.GetIntExtra(BatteryManager.ExtraLevel, -1);
                        var scale = intent.GetIntExtra(BatteryManager.ExtraScale, -1);
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
                    using (var intent = Application.Context.RegisterReceiver(null, filter))
                    {
                        var status = intent.GetIntExtra(BatteryManager.ExtraStatus, -1);
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
                    using (var intent = Application.Context.RegisterReceiver(null, filter))
                    {
                        var chargePlug = intent.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                        
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

        public IList<(string Key, string Value, string Description)> AdditionalInformation
        {
            get
            {
                if (!CheckBatteryPermissions())
                    throw new CanaryException(exceptionMessage);
                
                var data = new List<(string, string, string)>();
                data.Add((nameof(BatteryManager.ExtraTemperature), GetBatteryTemperature().ToString(".##"), "containing the current battery temperature in celsius"));
                data.Add((nameof(BatteryManager.ExtraTechnology), GetBatteryTechnology(), "describing the technology of the current battery"));
                data.Add((nameof(BatteryManager.ExtraVoltage), GetBatteryVoltage().ToString(".##"), "containing the current battery voltage level"));
                return data;
            }
        }

        #region private
        float GetBatteryTemperature()
        {
            using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
            {
                using (var intent = Application.Context.RegisterReceiver(null, filter))
                {
                    return ((float)intent.GetIntExtra(BatteryManager.ExtraTemperature, 0)) / 10;
                }
            }
        }

        string GetBatteryTechnology()
        {
            using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
            {
                using (var intent = Application.Context.RegisterReceiver(null, filter))
                {
                    return intent.GetStringExtra(BatteryManager.ExtraTechnology);
                }
            }
        }

        float GetBatteryVoltage()
        {
            using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
            {
                using (var intent = Application.Context.RegisterReceiver(null, filter))
                {
                    return intent.GetIntExtra(BatteryManager.ExtraVoltage, -1);
                }
            }
        }

        bool CheckBatteryPermissions()
        {
            var permission = Android.Manifest.Permission.BatteryStats;
            var res = Application.Context.CheckCallingOrSelfPermission(permission);
            return res == Permission.Granted;
        }
        #endregion
    }
}