using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ReblocAndroid.Models;

namespace ReblocAndroid.Adapters
{
    public class VendorAdapter : RecyclerView.Adapter
    {

        private List<Vendor> data;
        public event EventHandler<VendorAdapterClickEventArgs> ItemClick;
        public event EventHandler<VendorAdapterClickEventArgs> ItemLongClick;


        public VendorAdapter(List<Vendor> data)
        {
            this.data = data;

        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            var id = Resource.Layout.activity_main_grid_item;
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new JobsAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = data[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as JobsAdapterViewHolder;
            holder.Name.Text = data[position].Name;
            holder.Location.Text = data[position].Location;

        }

        public override int ItemCount => data.Count;

        void OnClick(VendorAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(VendorAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class JobsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; set; }
        public TextView Location { get; set; }
        public TextView PostedDate { get; set; }

        public JobsAdapterViewHolder(View itemView, Action<VendorAdapterClickEventArgs> clickListener,
                            Action<VendorAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.mgrid_title);
            Location = itemView.FindViewById<TextView>(Resource.Id.mgrid_location);

            itemView.Click += (sender, e) => clickListener(new VendorAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new VendorAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class VendorAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
    
}