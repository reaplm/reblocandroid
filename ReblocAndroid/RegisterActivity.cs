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

namespace ReblocAndroid
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        private EditText fName;
        private EditText lName;
        private EditText phone;
        private EditText email;
        private EditText password;
        private Button registerButton;
        private CheckBox rememberMeCheck;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_register);

            Title = "Register";

            fName = FindViewById<EditText>(Resource.Id.register_fname);
            lName = FindViewById<EditText>(Resource.Id.register_lname);
            phone = FindViewById<EditText>(Resource.Id.register_phone);
            email = FindViewById<EditText>(Resource.Id.register_email);
            password = FindViewById<EditText>(Resource.Id.register_password);
            phone = FindViewById<EditText>(Resource.Id.register_phone);
            registerButton = FindViewById<Button>(Resource.Id.register_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.register_rememberme);

            registerButton.Click += RegisterButton_Click;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}