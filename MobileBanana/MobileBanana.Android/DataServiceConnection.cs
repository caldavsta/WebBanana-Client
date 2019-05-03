using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MobileBanana.Droid
{
    public class DataServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public bool IsConnected { get; private set; }

        public DataServiceBinder Binder
        {
            get;
            set;
        }

        public DataServiceConnection()
        {
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var dataServiceBinder = service as DataServiceBinder;
            if (dataServiceBinder != null)
            {
                Binder = (DataServiceBinder)service;
                IsConnected = this.Binder != null;
            }

            string message = "onServiceConnected - ";
            Log.Debug("DataServiceConnection", $"OnServiceConnected {name.ClassName}");

            if (IsConnected)
            {
                message = message + " bound to service " + name.ClassName;
                //mainActivity.UpdateUiForBoundService();
            }
            else
            {
                message = message + " not bound to service " + name.ClassName;
                //mainActivity.UpdateUiForUnboundService();
            }

            Log.Debug("DataServiceConnection", message);
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            IsConnected = false;
            Binder = null;
            Log.Debug("DataService", "OnServiceDisconnected called");
        }
    }
}