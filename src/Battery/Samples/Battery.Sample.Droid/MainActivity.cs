using Android.App;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.App;

namespace Battery.Sample.Droid
{
    [Activity(Label = "Battery.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { 
                button.Text = $"{count++} clicks!"; 
            };

            CheckBatteryPermission();

            Sample.Core.Core.Instance = new Canary.Battery.Droid.CnrBattery();
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
                    Sample.Core.Core.Instance = new Canary.Battery.Droid.CnrBattery();
                }
            }
        }
    }
}

