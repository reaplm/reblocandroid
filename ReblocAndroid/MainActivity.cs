using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Support.V7.Widget;
using Adapters;
using System;
using ReblocAndroid.Models;
using System.Collections.Generic;
using Android.Content;
using Firebase;
using Firebase.Firestore;
using Android.Support.V4.View;
using Firebase.Auth;

namespace ReblocAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private DrawerLayout drawerLayout;
        private RecyclerView mainGrid;

        private MainGridAdapter mGridAdapter;
        List<GridItem> mainGridItems;

        int[] mainGridIcons = { Resource.Drawable.veges_40, Resource.Drawable.thanksgiving_turkey_40,
            Resource.Drawable.bed_40, Resource.Drawable.restaurant_40, Resource.Drawable.taxi_40,
            Resource.Drawable.milk_bottle_40, Resource.Drawable.potted_plant_40, Resource.Drawable.easel_40,
            Resource.Drawable.facial_mask_40, Resource.Drawable.pet_40, Resource.Drawable.wedding_cake_40,
            Resource.Drawable.worker_40, Resource.Drawable.feeding_chicken_40, Resource.Drawable.images_40,
            Resource.Drawable.sheep_40,Resource.Drawable.truck_40, Resource.Drawable.tractor_40,
            Resource.Drawable.concert_40
        };

        string[] mainGridText = { "Fruit And Veges", "Meat", "Accomodation", "Restaurants", "Cab", "Dairy",
            "Plants/Flowers", "Art", "Beauty", "Pets", "Event Planners", "Construction Workers",
            "Livestock Feed", "Photographers", "Livestock", "Water", "farming", "entertainment"
        };
        private FirebaseApp app;
        private FirebaseFirestore db;
        private FirebaseAuth auth;

        private const int REG_REQUEST_ID = 1001;
        private const int LOGIN_REQUEST_ID = 1002;

        static Result RESULT_OK = Result.Ok;
        static Result RESULT_CANCELED = Result.Canceled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.drawer_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Setup Navigation Drawer 
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            //Setup MainGrid
            mainGridItems = GetGridItems();

            mGridAdapter = new MainGridAdapter(mainGridItems);
            mGridAdapter.ItemClick += OnItemClick;

            mainGrid = FindViewById<RecyclerView>(Resource.Id.main_grid_view);
            mainGrid.SetLayoutManager(new GridLayoutManager(this, 3));
            mainGrid.SetAdapter( mGridAdapter);

            app = FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.GetInstance(app);

            //Setup Navigation View
            SetNavigationViewListener();

            //Setup UI
            UpdateUI();
        }

        private List<GridItem> GetGridItems()
        {
            List<GridItem> gridItems = new List<GridItem>();

            for (int i = 0; i < mainGridIcons.Length; i++)
            {
                gridItems.Add(new GridItem
                {
                    Name = mainGridText[i],
                    Resource = mainGridIcons[i]
                });
            }

            return gridItems;
        }
        /// <summary>
        /// MainGridItem Onclick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClick(object sender, MainGridAdapterClickEventArgs e)
        {
            //start a new activity
            var item = mainGridItems[e.Position];

            Intent intent = new Intent(this, typeof(MainGridDetailActivity));
            intent.PutExtra("Name", item.Name);

            StartActivity(intent);
        }

        private void SetNavigationViewListener()
        {
            NavigationView navigationView = drawerLayout.FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.action_login:
                    if (auth.CurrentUser == null) { SignIn(); }
                    else SignOut();
                    break;
            }
            drawerLayout.CloseDrawer(GravityCompat.Start, true);

            return true;
        }
        /// <summary>
        /// Sign In
        /// Starts LoginActivity
        /// </summary>
        void SignIn()
        {
            Intent intent = new Intent(this, typeof(LoginActivity));

            StartActivityForResult(intent, 1);
        }
        /// <summary>
        /// Sign out
        /// </summary>
        private void SignOut()
        {
            auth.SignOut();
            Toast.MakeText(this, "Sign Out Successful!", ToastLength.Long).Show();
            UpdateUI();
        }
        /// <summary>
        /// On return from activity
        /// 1: LoginActivity
        /// </summary>
        /// <param name="requestCode">Code to identify activity</param>
        /// <param name="resultCode">Success/Fail code</param>
        /// <param name="data">Returned Data</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //LoginActivity
            if (requestCode == 1002)
            {
                if (resultCode == RESULT_OK)
                {
                    bool isLoggedIn = data.GetBooleanExtra("isLoggedIn", false);

                    if (isLoggedIn)
                    {
                        UpdateUI();
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, data.GetStringExtra("message"), ToastLength.Long).Show();
                    }
                }
                else if (resultCode == RESULT_CANCELED)
                {
                    ;
                }
            }
        }
        /// <summary>
        /// Update user interface on login or logout
        /// </summary>
        private void UpdateUI()
        {
            NavigationView navigationView = drawerLayout.FindViewById<NavigationView>(Resource.Id.nav_view);

            IMenu menu = navigationView.Menu;
            IMenuItem menuItem = menu.FindItem(Resource.Id.action_login);

            View navHeader = navigationView.GetHeaderView(0);

            if (auth.CurrentUser != null)
            {
                menuItem.SetTitle("Log Out");
                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = auth.CurrentUser.Email;
            }
            else
            {
                menuItem.SetTitle("Log In");
                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = "";
            }
        }
    }
}