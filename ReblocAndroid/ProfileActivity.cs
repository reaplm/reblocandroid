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
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Org.Apache.Http.Authentication;
using ReblocAndroid.Adapters;
using ReblocAndroid.Models;

namespace ReblocAndroid
{
    [Activity(Label = "Account")]
    public class ProfileActivity : AppCompatActivity
    {
        private TextView name;
        private TextView email;
        private TextView phone;


        private FirebaseApp app;
        private FirebaseAuth auth;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_profile);

            name = FindViewById<TextView>(Resource.Id.profile_name_text);
            email = FindViewById<TextView>(Resource.Id.profile_email_text);
            phone = FindViewById<TextView>(Resource.Id.profile_phone_text);


            //get current user
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);

            var user = auth.CurrentUser;
            name.Text = "Pearl Molefe";
            email.Text = user.Email;
            phone.Text = "71406569";


            
        }
    }
}