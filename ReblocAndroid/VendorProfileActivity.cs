using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ReblocAndroid
{
    [Activity(Label = "VendorRegisterActivity")]
    public class VendorProfileActivity : AppCompatActivity
    {
        private ImageView profileImage;
        private ImageView logoImage;
        private TextView profileName;
        private TextView summary;
        private TextView phone;
        private TextView email;
        private ImageButton nameEdit;
        private ImageButton summaryEdit;
        private Spinner category;
        private GridLayout categoryLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_vendor_profile);

            //Card1
            profileImage = FindViewById<ImageView>(Resource.Id.vprofile_image);
            logoImage = FindViewById<ImageView>(Resource.Id.vprofile_logo); 
            profileName = FindViewById<TextView>(Resource.Id.vprofile_name);
            nameEdit = FindViewById<ImageButton>(Resource.Id.vprofile_name_edit);

            //Card2
            summary = FindViewById<TextView>(Resource.Id.vprofile_summary);
            summaryEdit = FindViewById<ImageButton>(Resource.Id.vprofile_summary_edit);

            //card3
            category = FindViewById<Spinner>(Resource.Id.vprofile_category);
            var adapter = ArrayAdapter<string>.CreateFromResource(
            this, Resource.Array.vendor_category, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            category.Adapter = adapter;

            categoryLayout = FindViewById<GridLayout>(Resource.Id.vprofile_category_layout);

            //card4
            phone = FindViewById<TextView>(Resource.Id.vprofile_phone);
            email = FindViewById<TextView>(Resource.Id.vprofile_email);


        }
    }
}