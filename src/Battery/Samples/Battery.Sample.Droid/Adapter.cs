using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Widget;

namespace Battery.Sample.Droid
{
    public class Adapter : BaseAdapter<KeyValuePair<string, string>>
    {
        Dictionary<string, string> _data;

        public Adapter(Dictionary<string, string> data)
        {
            _data = data;
        }

        public override KeyValuePair<string, string> this[int position] => _data.ToList().ElementAt(position);

        public override int Count => _data.Count;

        public override long GetItemId(int position)
        {
            return (long)position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var itemView = (LinearLayout)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell, parent, false);
            var titleView = itemView.FindViewById<TextView>(Resource.Id.title);
            var valueView = itemView.FindViewById<TextView>(Resource.Id.value);
            titleView.Text = $"{_data.ToList().ElementAt(position).Key}: " ;
            valueView.Text = _data.ToList().ElementAt(position).Value;
            return itemView;
        }
    }
}
