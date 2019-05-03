using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using VoiceMeeterClasses;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MobileBanana
{
    public class VoiceMeeterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsUpdatingFromServer { get; set; }

        public VoiceMeeterViewModel()
        {

        }

        private bool sendVoiceMeeterSettingsToServer = false;
        public bool SendVoiceMeeterSettingsToServer
        {
            get
            {
                return sendVoiceMeeterSettingsToServer;
            }
            set
            {
                if (sendVoiceMeeterSettingsToServer != value)
                {

                    Log.Warning("VoiceMeeterViewModel", "SendVoiceMeeterSettingsToServer has changed");
                    sendVoiceMeeterSettingsToServer = value;

                    OnPropertyChanged();
                }
            }
        }

        private VoiceMeeter voiceMeeter;
        public VoiceMeeter VoiceMeeter
        {
            get
            {
                return voiceMeeter;
            }
            set
            {
                if (voiceMeeter == null || !voiceMeeter.Equals(value))
                {
                    Log.Warning("VoiceMeeterViewModel", "VoiceMeeter has changed");
                    voiceMeeter = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
