using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Storage;
using ReblocAndroid.Models;
using Square.Picasso;
using static ReblocAndroid.Adapters.VendorAdapterViewHolder;

namespace ReblocAndroid.Adapters
{
    public class VendorAdapter : RecyclerView.Adapter
    {

        private List<Vendor> data;
        public event EventHandler<VendorAdapterClickEventArgs> ItemClick;
        public event EventHandler<VendorAdapterClickEventArgs> ItemLongClick;

        private Random rand = new Random();

        private const string directory = "thumbnails";

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

            var vh = new VendorAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = data[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as VendorAdapterViewHolder;
            holder.Name.Text = data[position].Name;
            holder.Location.Text = data[position].Location;

            //StaggeredGridLayoutManager.LayoutParams layoutParams = (StaggeredGridLayoutManager.LayoutParams)holder.ItemView.LayoutParameters;
            holder.Image.LayoutParameters.Height = RandomInt(250, 300);


            Picasso.Get().LoggingEnabled = true;
            Picasso.Get().Load(data[position].Thumbnail).Error(Resource.Drawable.image_100).Into(holder.Image);

        }
       
        /// <summary>
        /// Generate random integer between min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int RandomInt(int min, int max)
        {
            return rand.Next(max - min + min) + min;
        }
        public override int ItemCount => data.Count;

        void OnClick(VendorAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(VendorAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class VendorAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; set; }
        public TextView Location { get; set; }
        public TextView PostedDate { get; set; }
        public ImageView Image { get; set; }

        public VendorAdapterViewHolder(View itemView, Action<VendorAdapterClickEventArgs> clickListener,
                            Action<VendorAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.mgrid_title);
            Location = itemView.FindViewById<TextView>(Resource.Id.mgrid_location);
            Image = itemView.FindViewById<ImageView>(Resource.Id.mgrid_image);

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