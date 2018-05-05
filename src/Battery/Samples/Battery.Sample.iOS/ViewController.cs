using Battery.Sample.Core;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UIKit;

namespace Battery.Sample.iOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            TableView.Source = new Source(GetData());
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        Dictionary<string, string> GetData()
        {
            var batteryService = new BatteryService();
            var data = new Dictionary<string, string>();
            data.Add(nameof(batteryService.IsCharging), batteryService.IsCharging.ToString());
            data.Add(nameof(batteryService.BatteryLevel), batteryService.BatteryLevel.ToString());
            data.Add("BatteryState", batteryService.BatteryState);
            data.Add("PowerType", batteryService.PowerType);

            try
            {
                foreach (var item in batteryService.AddInfo)
                {
                    data.Add(item.Title, item.Value);
                }
            }
            catch (NotImplementedException ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return data;
        }
    }

    public class Source : UITableViewSource
    {
        KeyValuePair<string, string>[] _data;

        public Source(Dictionary<string, string> data)
        {
            _data = data.ToArray();
        }

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
            return 44f;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (TableCell)tableView.DequeueReusableCell("Cell");
            if (cell != null)
            {
                cell.PublicLabel.Text = $"{_data[indexPath.Row].Key}: {_data[indexPath.Row].Value}";
            }
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _data.Length;
        }
    }
}
