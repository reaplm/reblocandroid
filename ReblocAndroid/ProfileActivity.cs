using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Storage;
using Java.IO;
using Java.Util;
using Org.Apache.Http.Authentication;
using ReblocAndroid.Adapters;
using ReblocAndroid.Models;
using Square.Picasso;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using File = Java.IO.File;

namespace ReblocAndroid
{
    [Activity(Label = "Account")]
    public class ProfileActivity : AppCompatActivity, IOnSuccessListener, IOnCompleteListener, IDialogInterfaceOnClickListener
    {
       
        private TextView resetPassword;
        private TextView displayName;
        private ImageView profilePic;
        private ListView listView;

        private FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseFirestore db;
        private FirebaseStorage storage;

        private string uid;

        const int TAKE_PHOTO_REQ = 100;

        private string imagePath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_profile);

            //Setup firebase and firestore
            app = FirebaseApp.Instance;
            auth = FirebaseAuth.GetInstance(app);
            db = FirebaseFirestore.GetInstance(app);
            storage = FirebaseStorage.GetInstance(app);

            var name = Global.FName == string.Empty ? auth.CurrentUser.Email : Global.FName + " " + Global.LName;
            //ListView
            listView = FindViewById<ListView>(Resource.Id.profile_listview);
            List<ListItem> listData = new List<ListItem>
            {
                new ListItem { Heading = "NAME", SubHeading = name },
                new ListItem { Heading = "EMAIL", SubHeading = auth.CurrentUser.Email },
                new ListItem { Heading = "PHONE", SubHeading = Global.Phone == string.Empty ? "" : Global.Phone },
                new ListItem { Heading = "ACCOUNT", SubHeading = Global.UserType == string.Empty ? "Customer" : Global.UserType }
            };
            listView.Adapter = new ListAdapter(this, listData);
            listView.AddFooterView(new View(this));


            displayName = FindViewById<TextView>(Resource.Id.profile_name);
            displayName.Text = name;

            profilePic = FindViewById<ImageView>(Resource.Id.profile_pic);
            Picasso.Get().LoggingEnabled = true;
            Picasso.Get().Load(new File(Global.PhotoUrl)).Placeholder(Resource.Drawable.female_user_256)
                .Error(Resource.Drawable.female_user_256)
                .Into(profilePic);

            profilePic.Click += ProfilePic_Click;

        }

        private void ProfilePic_Click(object sender, EventArgs e)
        {
            //Open Dialog
            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);

            alertBuilder.SetTitle("Add Photo");
            alertBuilder.SetItems(Resource.Array.upload_photo, this);

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAccount_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_delete_account, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var email = view.FindViewById<EditText>(Resource.Id.dialog_delete_email);
            var password = view.FindViewById<EditText>(Resource.Id.dialog_delete_password);


            alertBuilder.SetTitle("Delete Account")
                .SetPositiveButton("Submit", delegate
                {
                try
                {

                    //update current user
                    var user = auth.CurrentUser;
                    if (user != null)
                    {
                            uid = user.Uid;

                            //delete from auth
                            var reauth = auth.CurrentUser.ReauthenticateAsync(EmailAuthProvider
                                .GetCredential(email.Text, password.Text)).ContinueWith(task => {
                                    if (task.IsCompletedSuccessfully)
                                    {
                                        Task result = user.Delete().AddOnCompleteListener(this);
                                        Toast.MakeText(this, "Yeah!", ToastLength.Short).Show();
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, "Failed to reauthenticate account.", ToastLength.Short).Show();
                                    }

                                });

                          
                        }
                    }
                    catch(Exception ex)
                    {
                        Toast.MakeText(this, "Sorry, an error occured during delete", ToastLength.Short).Show();
                    }
                    

                })
                .SetNegativeButton("No", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPassword_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_reset_password, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var newPassword = view.FindViewById<EditText>(Resource.Id.dialog_reset_pass);
            var passwordRpt = view.FindViewById<EditText>(Resource.Id.dialog_reset_pass_rpt);
            var passwordError = view.FindViewById<TextView>(Resource.Id.dialog_reset_pass_error);

            alertBuilder.SetTitle("Reset Password")
                .SetPositiveButton("Submit", delegate
                {
                    try
                    {
                        var user = auth.CurrentUser.UpdatePassword(newPassword.Text);

                        Toast.MakeText(this, "Done!", ToastLength.Long).Show();

                        //signout
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "An error occured. Sorry.", ToastLength.Long).Show();
                    }

                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();

            var submit = alertDialog.GetButton((int)DialogButtonType.Positive);
            submit.Click += delegate
            {
                if (!passwordRpt.Text.Equals(newPassword.Text))
                {
                    passwordError.Text = "Passwords must match";
                }
                else
                {
                    //update current user
                    var user = auth.CurrentUser;
                    if(user != null)
                    {
                        user.UpdatePassword(newPassword.Text);

                        Toast.MakeText(this, "Done!", ToastLength.Long).Show();
                    }
                    else { Toast.MakeText(this, "You are not logged in!",ToastLength.Long).Show(); }

                    alertDialog.Dismiss();
                }
            };

        }

        private void Submit_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dialog for editing phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPhone_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_profile_phone, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var editPhone = view.FindViewById<EditText>(Resource.Id.dialog_edit_phone);
           // editPhone.Text = phone.Text;

            alertBuilder.SetTitle("Edit Phone")
                .SetPositiveButton("Submit", delegate
                {
                    Toast.MakeText(this, "You clicked Submit!", ToastLength.Short).Show();

                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();

        }
        /// <summary>
        /// Dialog for editing first and last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditName_Click(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.dialog_profile_name, null);

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetView(view);

            var fName = view.FindViewById<EditText>(Resource.Id.dialog_edit_fname);
            var lName = view.FindViewById<EditText>(Resource.Id.dialog_edit_lname);

            //string[] fullName = name.Text.Split(" ");
            //fName.Text = fullName[0];
            //lName.Text = fullName[1];

            alertBuilder.SetTitle("Edit Name")
                .SetPositiveButton("Submit", delegate
                {
                    try
                    {
                        //get current user
                        if (auth.CurrentUser != null)
                        {
                            var document = db.Collection("users").Document(auth.CurrentUser.Uid);
                            var data = new Dictionary<string, Java.Lang.Object>();
                            data.Add("FName", fName.Text);
                            data.Add("LName", lName.Text);

                            document.Update((IDictionary<string, Java.Lang.Object>)data);

                            Toast.MakeText(this, "Done!", ToastLength.Long).Show();


                        }
                        else { Toast.MakeText(this, "Something went wrong. Sorry.", ToastLength.Long).Show(); }
                    }
                    catch(Exception ex)
                    {
                        Toast.MakeText(this, "Failed to update. Sorry.", ToastLength.Long).Show(); 
                    }
                    

                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertBuilder.Dispose();

                });

            AlertDialog alertDialog = alertBuilder.Create();
            alertDialog.Show();
        }
        /// <summary>
        /// Success for upload of profile image
        /// </summary>
        /// <param name="result"></param>
        public void OnSuccess(Java.Lang.Object result)
        {
            Toast.MakeText(this, "Profile changed successfully!", ToastLength.Long).Show();
        }
        /// <summary>
        /// Delete user listener
        /// </summary>
        /// <param name="task"></param>
        public void OnComplete(Task task)
        {
            var result = task.Result;

            if (task.IsSuccessful)
            {
                //Delete user details from firestore
                var docReference = db.Collection("users").Document(uid);

                if (docReference != null)
                {
                    var delete = docReference.Delete();
                    Toast.MakeText(this, "Awesome!", ToastLength.Long).Show();
                }

            }
            else
            {
                if(task.Exception != null)
                {
                    //Create log entry
                    Toast.MakeText(this, "Failed due to an exception", ToastLength.Long).Show();
                }
                else
                {
                    //Create a log entry
                    Toast.MakeText(this, "Couldn't delete. Sorry.", ToastLength.Long).Show();
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="which"></param>
        public void OnClick(IDialogInterface dialog, int position)
        {
            switch (position)
            {
                case 0:
                    UploadFromCamera();
                    break;
                case 1:
                    Toast.MakeText(this, "You clicked position 1", ToastLength.Long).Show();
                    break;
                case 2:
                    dialog.Dismiss();
                    break;
            }
        }

        private void UploadFromCamera()
        {

            try
            {
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    StartActivityForResult(intent, TAKE_PHOTO_REQ);
       
            }

            catch(Exception ex)
            {
                System.Console.WriteLine("Error taking profile picture: " + ex); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //LoginActivity
            if (requestCode == TAKE_PHOTO_REQ && data != null)
            {
                if (resultCode == Result.Ok)
                {
                   

                    try
                    {
                        Bitmap srcBitmap = (Bitmap)data.Extras.Get("data");
                        profilePic.SetImageBitmap(srcBitmap);
                        
                        var sdpath = Application.Context.GetExternalFilesDir(null).AbsolutePath;

                        File folder = new File(sdpath + File.Separator + "Profile Images");
                        
                        if (!folder.Exists())
                        {
                            folder.Mkdirs();
                        }
  
                        File file = new File(folder.AbsolutePath, auth.CurrentUser.Uid + ".jpg");


                        var stream = new FileStream(file.AbsolutePath, FileMode.Create);
                        srcBitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);

                        //Upload to firestore
                        //var task = storage.Reference.Child("profile/").Child(auth.Uid).PutFile(Android.Net.Uri.FromFile(new File(path)));
                        var reference = db.Collection("users").Document(auth.CurrentUser.Uid);
                        reference.Update("PhotoUrl", file.AbsolutePath);

                        profilePic.SetImageBitmap(srcBitmap);
                        stream.Flush();
                        stream.Close();


                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error saving profile image: " + ex);
                    }
                    finally
                    {
                        

                    }

                }
                else if (resultCode == Result.Canceled)
                {
                    ;
                }
            }
        }

    }
}