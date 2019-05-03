using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MobileBanana
{
	public partial class App : Application
	{
        public bool IsVisible { get; set; }
        public App ()
		{
			InitializeComponent();
            MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
            Log.Warning("App", "Xamarin has called OnStart() caleb");
        }

		protected override void OnSleep ()
		{
            base.OnSleep();
		}

		protected override void OnResume ()
		{
            base.OnResume();
		}
	}
}
