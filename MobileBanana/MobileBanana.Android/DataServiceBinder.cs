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

namespace MobileBanana.Droid
{
    public class DataServiceBinder : Binder
    {
        public DataServiceBinder(DataService service)
        {
            Service = service;
        }

        public DataService Service { get; private set; }
    }
}