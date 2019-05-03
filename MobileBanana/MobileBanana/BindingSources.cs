using System.Collections.Generic;
using VoiceMeeterClasses;
using Xamarin.Forms;

namespace MobileBanana
{
   
    public class BindingSources
    {
        public static MainPage currentMainPage;

        public static Android.App.Activity currentMainActivity;


        public static float VoiceMeeterLevel { get; set; }

        public static VoiceMeeter VoiceMeeter;
    }
}
