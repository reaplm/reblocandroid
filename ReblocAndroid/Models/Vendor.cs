using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReblocAndroid.Models
{
    public class Vendor
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Location { set; get; }
        public List<string> Categories { set; get; }
    }
}