using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ReblocAndroid;
using ReblocAndroid.Models;
using System;
using System.Collections.Generic;

namespace Adapters
{
    public class MainGridAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MainGridAdapterClickEventArgs> ItemClick;
        public event EventHandler<MainGridAdapterClickEventArgs> ItemLongClick;
        private List<GridItem> data;

        public MainGridAdapter(List<GridItem> data)
        {
            this.data = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            var id = Resource.Layout.main_grid_item;
            View itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MainGridAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            var item = data[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as MainGridAdapterViewHolder;
            holder.Icon.SetImageResource(item.Resource);
            holder.IconText.Text = item.Name;

        }

        public override int ItemCount => data.Count;

        void OnClick(MainGridAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(MainGridAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class MainGridAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Icon;
        public TextView IconText;

        public MainGridAdapterViewHolder(View itemView, Action<MainGridAdapterClickEventArgs> clickListener,
                            Action<MainGridAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Icon = itemView.FindViewById<ImageView>(Resource.Id.main_grid_image);
            IconText = itemView.FindViewById<TextView>(Resource.Id.main_grid_text);

            itemView.Click += (sender, e) => clickListener(new MainGridAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new MainGridAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class MainGridAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}