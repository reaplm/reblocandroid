using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ReblocAndroid.Models;

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

        private FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseFirestore firestore;
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
        /// <summary>
        /// Firebase Registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            Intent result = new Intent();
            try
            {

                //Initiaize Firebase Authentication Service
                app = FirebaseApp.Instance;
                auth = FirebaseAuth.GetInstance(app);
                firestore = FirebaseFirestore.GetInstance(app);

                // Attempt registration and login
                await auth.CreateUserWithEmailAndPasswordAsync(email.Text, password.Text);

                if (auth.CurrentUser != null)//Success
                { 
                    //Add user to database. Log failures
                    var dictionary = new Dictionary<string, Java.Lang.Object>();
                    dictionary.Add("Uid", auth.CurrentUser.Uid);
                    dictionary.Add("Email", auth.CurrentUser.Email);
                    dictionary.Add("FName", fName.Text);
                    dictionary.Add("LName", lName.Text);
                    dictionary.Add("Phone", phone.Text);
                    dictionary.Add("Display", fName.Text);

                    firestore.Collection("users").Document(auth.CurrentUser.Uid).Set(dictionary);

                    result.PutExtra("isLoggedIn", true);
                    result.PutExtra("message", "Registration Successful!");
                    SetResult(Result.Ok, result);

                    //Toast.MakeText(this,"Login Successful!", ToastLength.Long).Show();
                }
                else //failed login
                {
                    result.PutExtra("isLoggedIn", false);
                    result.PutExtra("message", "Registration falied");
                    SetResult(Result.Ok, result);

                    //Toast.MakeText(this, "Login Failed", ToastLength.Long).Show();

                }

            }
            catch (Exception ex)
            {
                result.PutExtra("isLoggedIn", false);
                result.PutExtra("message", ex.Message);
                SetResult(Result.Ok, result);
                //Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
 
            }
            finally
            {
                //Close Activity 
                Finish();
            }
        }
       
    }
}