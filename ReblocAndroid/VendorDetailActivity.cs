using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace ReblocAndroid
{
    [Activity(Label = "VendorDetailActivity")]
    public class VendorDetailActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_vendor_detail);

            Intent intent = Intent;
            Title = intent.GetStringExtra("Title");

            FindViewById<TextView>(Resource.Id.vdetail_title).Text = intent.GetStringExtra("Name");
            FindViewById<TextView>(Resource.Id.vdetail_overview).Text = intent.GetStringExtra("Overview");
            FindViewById<TextView>(Resource.Id.vdetail_location).Text = intent.GetStringExtra("Location");

            var image = intent.GetStringExtra("Image");
            Picasso.Get().LoggingEnabled = true;
            Picasso.Get().Load(image).Error(Resource.Drawable.image_100).Into(FindViewById<ImageView>(Resource.Id.vdetail_image));


        }
    }
}