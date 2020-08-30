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
using Android.Media;
using ReblocAndroid.Adapters;
using System.Linq;
using Square.Picasso;
using Java.IO;

namespace ReblocAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, View.IOnClickListener, IEventListener
    {
        private DrawerLayout drawerLayout;
        private RecyclerView mainGrid;
        private RecyclerView mainHorizontal;

        private MainGridAdapter mGridAdapter;
        private HorizontalRvAdapter hGridAdapter;
        List<GridItem> mainGridItems;
        List<GridItem> horizontalRvItems;

        int[] horizontalGridIcons = { Resource.Drawable.vector_icon_1, Resource.Drawable.vector_icon_2,
            Resource.Drawable.vector_icon_3, Resource.Drawable.vector_icon_4, Resource.Drawable.vector_icon_5,
            Resource.Drawable.vector_icon_6, Resource.Drawable.vector_icon_7, Resource.Drawable.vector_icon_8, 
            Resource.Drawable.vector_icon_9, Resource.Drawable.vector_icon_10
        };

        string[] horizontalGridText = { "Fruity Jam", "Fruity Orchards", "Summer Popsicles", "Juicy Smoothies",
            "Vegan Foods", "Vegetable Food Jar", "Vege Farm", "All Natural", "Green Basket", "Healthy Food"
        };
        string[] mainGridText = { "Fruit And Veges", "Meat", "Accomodation", "Restaurants", "Cab", "Dairy",
            "Plants/Flowers", "Art", "Beauty", "Pets", "Event Planners", "Construction Workers",
            "Livestock Feed", "Photographers", "Livestock", "Water", "farming", "entertainment"
        };
        private FirebaseApp app;
        private FirebaseFirestore db;
        private FirebaseAuth auth;

        private ImageView profileImage;

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

            //Setup Horizontal RV
            horizontalRvItems = GetHorizontalRvItems();

            hGridAdapter = new HorizontalRvAdapter(horizontalRvItems);
            mainHorizontal = FindViewById<RecyclerView>(Resource.Id.main_horizontal_rv);
            mainHorizontal.SetAdapter(hGridAdapter);


            app = FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.GetInstance(app);
            db = FirebaseFirestore.GetInstance(app);

            //Fetch user details from server
            GetUserDetails();

            //Setup Navigation View
            SetNavigationViewListener();


            //Setup UI
            UpdateUI();
        }
        /// <summary>
        /// Fetch user's account detailsfrom firebase
        /// </summary>
        private void GetUserDetails()
        {
            //User is logged in
            if(auth != null)
            {
                CollectionReference collection = db.Collection("users");
                collection.WhereEqualTo("Uid", auth.Uid).AddSnapshotListener(this);
            }

        }

        /// <summary>
        /// Main grid
        /// </summary>
        /// <returns></returns>
        private List<GridItem> GetGridItems()
        {
            List<GridItem> gridItems = new List<GridItem>();

            gridItems.Add(new GridItem { Name = "Fruit And Veges", Resource = Resource.Drawable.veges_40, Category = "produce" });
            gridItems.Add(new GridItem { Name = "Meat", Resource = Resource.Drawable.thanksgiving_turkey_40, Category =  "meat" });
            gridItems.Add(new GridItem { Name = "Accomodation", Resource = Resource.Drawable.bed_40, Category =  "accomodation" });
            gridItems.Add(new GridItem { Name = "Restaurants", Resource = Resource.Drawable.restaurant_40, Category = "restaurants"});
            gridItems.Add(new GridItem { Name = "Dairy", Resource = Resource.Drawable.milk_bottle_40, Category = "dairy" });
            gridItems.Add(new GridItem { Name = "Cab", Resource = Resource.Drawable.taxi_40, Category = "transport" });
            gridItems.Add(new GridItem { Name = "Plants/Flowers", Resource = Resource.Drawable.potted_plant_40, Category =  "flowers"});
            gridItems.Add(new GridItem { Name = "Art", Resource = Resource.Drawable.easel_40, Category = "art" });
            gridItems.Add(new GridItem { Name = "Beauty", Resource = Resource.Drawable.facial_mask_40, Category =  "beauty" });
            gridItems.Add(new GridItem { Name = "Pets", Resource = Resource.Drawable.worker_40, Category = "pets" });
            gridItems.Add(new GridItem { Name = "Event Planners", Resource = Resource.Drawable.wedding_cake_40, Category = "events"});
            gridItems.Add(new GridItem { Name = "Construction Workers", Resource = Resource.Drawable.worker_40, Category = "construction"});
            gridItems.Add(new GridItem { Name = "Livestock Feed", Resource = Resource.Drawable.feeding_chicken_40, Category = "feeds"});
            gridItems.Add(new GridItem { Name = "Photographers", Resource = Resource.Drawable.images_40, Category = "photography" });
            gridItems.Add(new GridItem { Name = "Livestock", Resource = Resource.Drawable.tractor_40, Category = "livestock" });
            gridItems.Add(new GridItem { Name = "Water", Resource = Resource.Drawable.truck_40, Category = "water" });
            gridItems.Add(new GridItem { Name = "Farming", Resource = Resource.Drawable.tractor_40, Category =  "farming" });
            gridItems.Add(new GridItem { Name = "Entertainment", Resource = Resource.Drawable.concert_40, Category = "entertainment" });


            return gridItems;
        }
        private List<GridItem> GetHorizontalRvItems()
        {
            List<GridItem> gridItems = new List<GridItem>();

            for (int i = 0; i < horizontalGridIcons.Length; i++)
            {
                gridItems.Add(new GridItem
                {
                    Name = horizontalGridText[i],
                    Resource = horizontalGridIcons[i]
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
            intent.PutExtra("category", item.Category);

            StartActivity(intent);
        }

        private void SetNavigationViewListener()
        {
            NavigationView navigationView = drawerLayout.FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            var navHeader = navigationView.GetHeaderView(0);
            navHeader.SetOnClickListener(this);
           
        }
        public void OnClick(View view)
        {
            int id = view.Id;

            switch (id)
            {
                case Resource.Id.nav_header:
                    if (auth.CurrentUser != null)
                    {
                        if(Global.UserType == "vendor")
                        {
                            Intent intent = new Intent(this, typeof(VendorProfileActivity));
                            StartActivity(intent);
                        }
                        else 
                        {
                            Intent intent = new Intent(this, typeof(ProfileActivity));
                            StartActivity(intent);
                        }
                        
                    }
                    else
                    {
                        SignIn();
                    }
                    break;
            }
        }

        private void ProfileClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "You Clicked Profile", ToastLength.Long).Show();
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

            StartActivityForResult(intent, LOGIN_REQUEST_ID);
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
            if (requestCode == LOGIN_REQUEST_ID)
            {
                if (resultCode == RESULT_OK)
                {
                    bool isLoggedIn = data.GetBooleanExtra("isLoggedIn", false);

                    if (isLoggedIn)
                    {
                        GetUserDetails();
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

                //Update navigation header
                Picasso.Get().LoggingEnabled = true;
                Picasso.Get().Load(new File(Global.PhotoUrl)).Placeholder(Resource.Drawable.female_user_256)
                    .Error(Resource.Drawable.female_user_256)
                    .Into(navHeader.FindViewById<ImageView>(Resource.Id.nav_header_image));

                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = Global.FName == string.Empty ?
                    auth.CurrentUser.Email : Global.FName;

            }
            else
            {
                menuItem.SetTitle("Log In");
                navHeader.FindViewById<TextView>(Resource.Id.nav_header_text).Text = "";
                navHeader.FindViewById<ImageView>(Resource.Id.nav_header_image).SetImageResource(Resource.Drawable.female_user_256);

                Global.FName = string.Empty;
                Global.LName = string.Empty;
                Global.Phone = string.Empty;
                Global.PhotoUrl = string.Empty;
                Global.UserType = string.Empty;

            }
        }
        /// <summary>
        /// Listener for FetchUserDetails
        /// </summary>
        /// <param name="value"></param>
        /// <param name="error"></param>
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {

            if (error != null)
            {
                Toast.MakeText(this, "Failed to fetch user details", ToastLength.Long).Show();
                return;
            }

            var snapshot = (QuerySnapshot)value;
            if (snapshot.Documents.Count == 1)
            {
                var dictionary = snapshot.Documents.First().Data;

                //Update global variables
                Global.FName = dictionary.ContainsKey("FName") ? (string)dictionary["FName"] : string.Empty;
                Global.LName = dictionary.ContainsKey("LName") ? (string)dictionary["LName"] : string.Empty;
                Global.PhotoUrl = dictionary.ContainsKey("PhotoUrl") ? (string)dictionary["PhotoUrl"] : string.Empty;
                Global.Phone = dictionary.ContainsKey("Phone") ? (string)dictionary["Phone"] : string.Empty;
                Global.UserType = dictionary.ContainsKey("Type") ? (string)dictionary["Type"] : "customer";

                UpdateUI();

            }
            else
            {
                Toast.MakeText(this, "There was an error fetching user details", ToastLength.Long).Show();
            }
        }
    }
}