using Android.App;
using Android.Widget;
using Android.OS;
using SoC.Sample.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoC.Sample.Droid
{
    [Activity(Label = "SoC.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        ListView _listView;
        Dictionary<string, string> _data = new Dictionary<string, string>();

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var data = await GetData();

            _listView = FindViewById<ListView>(Resource.Id.listview);
            _listView.Adapter = new Adapter(data);
        }

        async Task<Dictionary<string, string>> GetData()
        {
            var socService = new SoCService();
            var data = new Dictionary<string, string>();
            data.Add(nameof(socService.Model), socService.Model);
            data.Add(nameof(socService.Cores), socService.Cores.ToString());
            data.Add(nameof(socService.MinFrequency), socService.MinFrequency.ToString());
            data.Add(nameof(socService.MaxFrequency), socService.MaxFrequency.ToString());
            var addData = await socService.GetAddInfo();
            foreach (var item in addData)
            {
                data.Add(item.Key, item.Value);
            }
            return data;
        }
    }
}

