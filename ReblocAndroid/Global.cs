﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReblocAndroid
{
    public class Global : Application
    {
        public static string FName { set; get; } = string.Empty;
        public static string LName { set; get; } = string.Empty;
        public static string PhotoUrl { set; get; } = string.Empty;
        public static string Phone { set; get; } = string.Empty;
        public static string UserType { set; get; } = string.Empty;
    }
}