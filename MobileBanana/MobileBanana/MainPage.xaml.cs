using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MobileBanana
{
    public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();
            BindingSources.currentMainPage = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            Log.Warning("MainPage", "OnAppearing() called");
            
        }
    }
}
