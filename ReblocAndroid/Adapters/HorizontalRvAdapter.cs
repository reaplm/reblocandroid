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
    public class HorizontalRvAdapter : RecyclerView.Adapter
    {
        public event EventHandler<HorizontalRvAdapterClickEventArgs> ItemClick;
        public event EventHandler<HorizontalRvAdapterClickEventArgs> ItemLongClick;
        private List<GridItem> data;

        public HorizontalRvAdapter(List<GridItem> data)
        {
            this.data = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            var id = Resource.Layout.activity_main_ho_item;
            View itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new HorizontalRvAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            var item = data[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as HorizontalRvAdapterViewHolder;
            holder.Icon.SetImageResource(item.Resource);
            holder.IconText.Text = item.Name;

        }

        public override int ItemCount => data.Count;

        void OnClick(HorizontalRvAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(HorizontalRvAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class HorizontalRvAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Icon;
        public TextView IconText;

        public HorizontalRvAdapterViewHolder(View itemView, Action<HorizontalRvAdapterClickEventArgs> clickListener,
                            Action<HorizontalRvAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Icon = itemView.FindViewById<ImageView>(Resource.Id.horizontal_rv_image);
            IconText = itemView.FindViewById<TextView>(Resource.Id.horizontal_rv_text);

            itemView.Click += (sender, e) => clickListener(new HorizontalRvAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new HorizontalRvAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class HorizontalRvAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}