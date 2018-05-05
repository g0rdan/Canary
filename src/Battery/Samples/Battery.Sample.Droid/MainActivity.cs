using Android.App;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using System.Collections.Generic;
using Battery.Sample.Core;

namespace Battery.Sample.Droid
{
    [Activity(Label = "Battery.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        ListView _listView;
        Dictionary<string, string> _data = new Dictionary<string, string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CheckBatteryPermission();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _listView = FindViewById<ListView>(Resource.Id.listview);
            _listView.Adapter = new Adapter(GetData());
        }

        Dictionary<string, string> GetData()
        {
            var batteryService = new BatteryService();
            var data = new Dictionary<string, string>();
            data.Add(nameof(batteryService.IsCharging), batteryService.IsCharging.ToString());
            data.Add(nameof(batteryService.BatteryLevel), batteryService.BatteryLevel.ToString());
            data.Add("BatteryState", batteryService.BatteryState);
            data.Add("PowerType", batteryService.PowerType);
            foreach (var item in batteryService.AddInfo)
            {
                data.Add(item.Title, item.Value);
            }
            return data;
        }

        void CheckBatteryPermission()
        {
            //var permission = PermissionChecker.CheckSelfPermission(this, Manifest.Permission.BatteryStats);
            if (PermissionChecker.CheckSelfPermission(this, Manifest.Permission.BatteryStats) != (int)Permission.Granted)
            {
                // Should we show an explanation?
                //if (ShouldShowRequestPermissionRationale(Manifest.Permission.BatteryStats))
                //{
                //    //ViewModel.ShowMessage("Нужно разрешить использование местоположения для пользования всеми доступными функциями", "Предупреждение");
                //}
                //else
                //{
                    // No explanation needed, we can request the permission.
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.BatteryStats }, 99);
                //}
                //return false;
            }
            else
            {
                //return true;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 99)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    //var percent = Core.BatteryService.GetBatteryPercentage();
                    _listView.Adapter = new Adapter(GetData());
                }
            }
        }
    }
}

