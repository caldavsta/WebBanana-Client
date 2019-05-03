using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Content;
using Xamarin.Forms.Internals;
using MobileBanana;
using Java.Interop;
using VoiceMeeterClasses;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MobileBanana.Droid
{
    [Activity(Label = "MobileBanana", Icon = "@mipmap/icon", Theme = "@style/FullScreenTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, View.IOnClickListener, SeekBar.IOnSeekBarChangeListener
    {
        private const int VoiceMeeterMaximumPollingDelay = 2000;
        DataServiceConnection dataServiceConnection;
        public static MainActivity Instance { get; private set; }
        public bool UserIsAdjustingGain = false;
        private bool ViewReferencesSet { get; set; } = false;


        public VoiceMeeterViewModel vm;
        SeekBar gain0;
        ToggleButton but_a1_0;
        ToggleButton but_a2_0;
        ToggleButton but_a3_0;
        ToggleButton but_b1_0;
        ToggleButton but_b2_0;
        ToggleButton but_mute_0;
        ToggleButton but_mono_0;
        ToggleButton but_solo_0;
        ProgressBar pb_0;
      

        public void UpdateUi(VoiceMeeter voiceMeeter)
        {
            if (ViewReferencesSet)
            {
                Log.Warning("MainActivity", "Update UI called");
                if (!UserIsAdjustingGain)
                {
                    gain0.Progress = Convert.ToInt32(voiceMeeter.Strips[0].Gain + 60);
                }
                pb_0.Progress = Convert.ToInt32(BindingSources.VoiceMeeterLevel * 1000);
                but_a1_0.Checked = voiceMeeter.Strips[0].A1;
                but_a2_0.Checked = voiceMeeter.Strips[0].A2;
                but_a3_0.Checked = voiceMeeter.Strips[0].A3;
                but_b1_0.Checked = voiceMeeter.Strips[0].B1;
                but_b2_0.Checked = voiceMeeter.Strips[0].B2;
                but_mute_0.Checked = voiceMeeter.Strips[0].Mute;
                but_mono_0.Checked = voiceMeeter.Strips[0].Mono;
                but_solo_0.Checked = voiceMeeter.Strips[0].Solo;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());

            Instance = this;
            SetContentView(Resource.Layout.VoiceMeeterLayout1);
            SetupViews();

            BindingSources.currentMainActivity = this;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (dataServiceConnection == null)
            {
                this.dataServiceConnection = new DataServiceConnection();
            }

            Log.Warning("MainActivity", "OnStart");

            Intent dataServiceIntent = new Intent(this, typeof(DataService));
            BindService(dataServiceIntent, dataServiceConnection, Bind.AutoCreate);
        }

        private void SetupViews()
        {
            gain0 = FindViewById(Resource.Id.gain0) as SeekBar;
            gain0.SetOnSeekBarChangeListener(this);

            gain0.Progress = 0;

            but_a1_0 = FindViewById(Resource.Id.A1_0) as ToggleButton;
            but_a2_0 = FindViewById(Resource.Id.A2_0) as ToggleButton;
            but_a3_0 = FindViewById(Resource.Id.A3_0) as ToggleButton;
            but_b1_0 = FindViewById(Resource.Id.B1_0) as ToggleButton;
            but_b2_0 = FindViewById(Resource.Id.B2_0) as ToggleButton;
            but_mute_0 = FindViewById(Resource.Id.Mute_0) as ToggleButton;
            but_solo_0 = FindViewById(Resource.Id.Solo_0) as ToggleButton;
            but_mono_0 = FindViewById(Resource.Id.Mono_0) as ToggleButton;
            pb_0 = FindViewById(Resource.Id.levels_0) as ProgressBar;
            ViewReferencesSet = true;
        }


        protected override void OnPause()
        {
            // todo: 

            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
            UnbindService(dataServiceConnection);
            Log.Warning("MainActivity", "OnStop Called");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Instance = null;
        }
        
        [Export]
        public void OnClick(View v)
        {
            string command = string.Empty;
            if (dataServiceConnection == null)
            {
                return;
            }
            switch (v.Id)
            {
                case Resource.Id.Mute_0:
                    command = "Strip[0].Mute = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].Mute);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.Solo_0:
                    command = "Strip[0].Solo = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].Solo);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.A1_0:
                    command = "Strip[0].A1 = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].A1);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.A2_0:
                    command = "Strip[0].A2 = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].A2);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.A3_0:
                    command = "Strip[0].A3 = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].A3);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.B1_0:
                    command = "Strip[0].B1 = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].B1);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.B2_0:
                    command = "Strip[0].B2 = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].B2);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
                case Resource.Id.Mono_0:
                    command = "Strip[0].Mono = " + Convert.ToInt32(!BindingSources.VoiceMeeter.Strips[0].Mono);
                    ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add(command);
                    break;
            }
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (MainActivity.Instance.UserIsAdjustingGain)
            {
                switch (seekBar.Id)
                {
                    case Resource.Id.gain0:
                        ((DataService)dataServiceConnection.Binder.Service).CommandQueue.Add("Strip[0].Gain = " + (progress - 60));
                        break;
                }
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            UserIsAdjustingGain = true;
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {

            UserIsAdjustingGain = false;
        }
    }




}

