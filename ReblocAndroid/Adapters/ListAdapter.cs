using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReblocAndroid.Models;

namespace ReblocAndroid.Adapters
{
    public class ListAdapter : BaseAdapter<ListItem>
    {

        Context context;
        List<ListItem> data;

        public ListAdapter(Context context, List<ListItem> data) : base()
        {
            this.context = context;
            this.data = data;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = data[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.profile_list_item, parent, false);

            view.FindViewById<TextView>(Resource.Id.profile_item_heading).Text = item.Heading;
            view.FindViewById<TextView>(Resource.Id.profile_item_sub_heading).Text = item.SubHeading;

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

        public override ListItem this[int position] => throw new NotImplementedException();
    }

    class ListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}