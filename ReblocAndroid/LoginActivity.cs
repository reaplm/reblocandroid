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

        private const int REG_REQUEST_ID = 1001;

        static Result RESULT_OK = Result.Ok;
        static Result RESULT_CANCELED = Result.Canceled;

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
        /// <summary>
        /// Start registration activity     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterLink_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));

            StartActivityForResult(intent, 1001);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Intent result callback
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //RegistrationActivity
            if (requestCode == 1001)
            {
                if (resultCode == RESULT_OK)
                {
                    bool isLoggedIn = data.GetBooleanExtra("isLoggedIn", false);

                    if (isLoggedIn)
                    {
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();

                    }
                    else
                    {
                        //UpdateUI();
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                }
                else if (resultCode == RESULT_CANCELED)
                {
                    ;
                }
            }
        }
    }
}