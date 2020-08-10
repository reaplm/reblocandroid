using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using Java.Lang;
using ReblocAndroid.Adapters;
using ReblocAndroid.Models;

namespace ReblocAndroid
{
    [Activity(Label = "@string/app_name")]
    public class MainGridDetailActivity : AppCompatActivity, IEventListener
    {
        private RecyclerView recyclerView;
        private VendorAdapter vendorAdapter;
        private List<Vendor> vendors;

        private FirebaseApp app;
        private FirebaseFirestore db;
        private FirebaseStorage storage;

        private SwipeRefreshLayout swipeRefreshLayout;
        private bool reload = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_main_grid);

            Intent intent = Intent;
            Title = intent.GetStringExtra("Name");
            //SetActionBar(android.)

            app = FirebaseApp.Instance;
            db = FirebaseFirestore.GetInstance(app);

            //Setup Views
            swipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.refreshView);
            swipeRefreshLayout.Refresh += delegate (object sender, System.EventArgs e)
            {
                reload = true;
                FetchCollection();
            };

            swipeRefreshLayout.Post(() => {
                swipeRefreshLayout.Refreshing = true;
                recyclerView.Clickable = false;
            });

            reload = true;
            vendors = new List<Vendor>();
            FetchCollection();
            vendorAdapter = new VendorAdapter(vendors);
            vendorAdapter.ItemClick += OnItemClick;

            recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_mgrid_rv);
            recyclerView.SetLayoutManager(new StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.Vertical));
            recyclerView.SetAdapter(vendorAdapter);
        }

        private void OnItemClick(object sender, VendorAdapterClickEventArgs e)
        {
            Toast.MakeText(this, "You Cliked!", ToastLength.Short).Show();
        }

        /// <summary>
        /// Fetch collection from firestore database
        /// </summary>
        private void FetchCollection()
        {

            try
            {
                CollectionReference collection = db.Collection("vendors");
                collection.AddSnapshotListener(this);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in FindAllAsync: " + ex.Message);
            }

        }
        /// <summary>
        /// Snapshot listener that is trigger by DB changes (added, modified, deleted)
        /// </summary>
        /// <param name="value">New Snapshot</param>
        /// <param name="error">Firebase exception object</param>
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            if (error != null)
            {
                //Log.w("TAG", "listen:error", error);
                return;
            }

            int count = 1;
            var snapshots = (QuerySnapshot)value;
            foreach (DocumentChange dc in snapshots.DocumentChanges)
            {
                var dictionary = dc.Document.Data;
                dictionary.Add("Id", dc.Document.Id);
                var vendor = ToObject(dictionary);



                switch (dc.GetType().ToString())
                {
                    case "ADDED":
                        //Log.d("TAG", "New Msg: " + dc.getDocument().toObject(Message.class));
                        vendors.Add(vendor);


                        break;

                    case "MODIFIED":
                        //find modified item and replace with new document
                        int index = vendors.FindIndex(x => x.Id == vendor.Id);
                        vendors.RemoveAt(index);
                        vendors.Insert(index, vendor);
                        break;
                    case "REMOVED":
                        //delete vendor
                        break;
                }
            }
            swipeRefreshLayout.Post(() => {
                swipeRefreshLayout.Refreshing = false;
                recyclerView.Clickable = true;
            });

            if (reload)
            {
                vendorAdapter.NotifyDataSetChanged(); //for updating adapter
                reload = false;
            }
            else
            {
                //make toast
                Toast toast = Toast.MakeText(this, "new vendors added", ToastLength.Long);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }

        }
        /// <summary>
        /// Convert DocumentSnapshot to Object
        /// </summary>
        /// <param name="source">Dictionary</param>
        /// <returns>Object</returns>
        public Vendor ToObject(IDictionary<string, Java.Lang.Object> source)
        {
            var newObject = new Vendor();
            var newObjectType = newObject.GetType();

            foreach (var item in source)
            {
                if (newObjectType.GetProperty(item.Key) != null)
                {
                    if (item.Value.GetType().Equals(typeof(Java.Lang.String)))
                    {
                        //covert Java.Lang.String to System.string
                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, item.Value.ToString(), null);

                    }
                    if (item.Value.GetType().Equals(typeof(Java.Util.Date)))
                    {
                        //covert Java.Util.Date to DateTime
                        Java.Util.Date dt = (Java.Util.Date)(item.Value);
                        DateTime dateValue = new DateTime(1970, 1, 1).AddMilliseconds(dt.Time).ToLocalTime();

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, dateValue, null);
                    }
                    if (item.Value.GetType().Equals(typeof(JavaList)))
                    {
                        //covert JavaList to List<>
                        var javaList = (JavaList)item.Value;
                        List<string> newList = new List<string>();

                        foreach (var listItem in javaList)
                        {
                            newList.Add(listItem.ToString());
                        }

                        newObjectType
                            .GetProperty(item.Key)
                            .SetValue(newObject, newList, null);
                    }
                }
            }

            return newObject;
        }
    }
}