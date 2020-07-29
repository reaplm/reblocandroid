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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : AppCompatActivity
    {
        private EditText email;
        private EditText password;
        private TextView registerLink;
        private Button loginButton;
        private CheckBox rememberMeCheck;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login);
            Title = "Login";

            email = FindViewById<EditText>(Resource.Id.email);
            password = FindViewById<EditText>(Resource.Id.password);
            registerLink = FindViewById<TextView>(Resource.Id.register_link);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            rememberMeCheck = FindViewById<CheckBox>(Resource.Id.login_rememberme);

            registerLink.Click += RegisterLink_Click;
            loginButton.Click += LoginButton_Click;
        }

        private void RegisterLink_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));

            StartActivity(intent);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}