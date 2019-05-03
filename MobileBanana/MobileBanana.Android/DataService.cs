using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VoiceMeeterClasses;

namespace MobileBanana.Droid
{
    [Service(Name = "com.onagainapps.VoiceBananaService")]
    public class DataService : Service
    {
        private const string VoiceMeeterUrl = "http://192.168.1.43:5090/voicemeeter/";
        private const string ParamIsDirtyUrl = "http://192.168.1.43:5090/voicemeeter/isdirty";
        private const string ScriptUrl = "http://192.168.1.43:5090/voicemeeter/script";
        private const string LevelsUrl = "http://192.168.1.43:5090/voicemeeter/level/0/0";
        private const int BasicPollingDelay = 90;
        private const int VoiceMeeterMaximumPollingDelay = int.MaxValue;
        private const int VoiceMeeterLevelsPollingDelay = 500;
        private static HttpClient _client = new HttpClient();


        public volatile List<string> CommandQueue = new List<string>();

        public bool ClientIsBound { get; set; } = true;
        public IBinder Binder { get; private set; }
        private Thread workerThread;
        

        public override IBinder OnBind(Intent intent)
        {
            this.Binder = new DataServiceBinder(this);
            ClientIsBound = true;
            return Binder;
        }

        public override bool OnUnbind(Intent intent)
        {
            ClientIsBound = false;
            Log.Warning("DataService", "DataService Unbound");
            return base.OnUnbind(intent);
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Log.Warning("DataService", "DataService started");

            workerThread = new Thread(async () =>
            {
                Stopwatch basicPollingStopwatch = new Stopwatch(); // measures time since last poll
                basicPollingStopwatch.Start();

                Stopwatch voiceMeeterMaximumDelayStopWatch = new Stopwatch();// measures time since last automatically-called manual update to voicemeeter display controls
                voiceMeeterMaximumDelayStopWatch.Start();

                Stopwatch voiceMeeterLevelsStopwatch = new Stopwatch();
                voiceMeeterLevelsStopwatch.Start();

                bool requestUiUpdate = false;
                string isDirtyResponse = string.Empty;
                bool isDirty = false;
                string script = string.Empty;

                while (true)
                {
                    isDirty = false;
                    isDirtyResponse = string.Empty;
                    if (ClientIsBound && MainActivity.Instance != null )
                    {
                        if (basicPollingStopwatch.ElapsedMilliseconds > BasicPollingDelay)
                        {
                            basicPollingStopwatch.Restart();

                            if (BindingSources.VoiceMeeter == null)
                            {
                                BindingSources.VoiceMeeter = await GetVoiceMeeterFromServer();
                                requestUiUpdate = true;
                            } else
                            {
                                if (CommandQueue.Count > 0)
                                {
                                    script = string.Empty;
                                    foreach (string command in CommandQueue)
                                    {
                                        script += command + ";";
                                    }
                                    Log.Warning("DataService", "Sending script to server " + script);
                                    CommandQueue.Clear();
                                    await UpdateParametersWithScript(script);
                                    requestUiUpdate = true;
                                } else
                                {
                                    isDirtyResponse = await UpdateFromServer(ParamIsDirtyUrl);
                                    isDirty = Convert.ToBoolean(isDirtyResponse);

                                    if (isDirty || voiceMeeterMaximumDelayStopWatch.ElapsedMilliseconds > VoiceMeeterMaximumPollingDelay)
                                    {
                                        Log.Warning("DataService", "Params are Dirty, haven't been Downloaded yet, or are ready to be updated because they're old. Updating them...");
                                        BindingSources.VoiceMeeter = await GetVoiceMeeterFromServer();
                                        requestUiUpdate = true;
                                        voiceMeeterMaximumDelayStopWatch.Restart();
                                    }

                                    if (voiceMeeterLevelsStopwatch.ElapsedMilliseconds > VoiceMeeterLevelsPollingDelay)
                                    {
                                        voiceMeeterLevelsStopwatch.Restart();
                                        UpdateVoiceMeeterLevelsFromServer();
                                        requestUiUpdate = true;
                                    }

                                    if (requestUiUpdate)
                                    {
                                        if (MainActivity.Instance != null)
                                        {
                                            // TODO: replace the call here to ipdate the UI
                                            MainActivity.Instance.RunOnUiThread(() => MainActivity.Instance.UpdateUi(BindingSources.VoiceMeeter));
                                            requestUiUpdate = false;
                                        }
                                        else
                                        {
                                            this.StopSelf();
                                        }
                                    }
                                }
                            }

                        }

                    }

                }
            });
            workerThread.Start();
        }
    
        private async Task<string> UpdateFromServer(string Url)
        {
             string result = "";
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _client.GetAsync(Url);
                result = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Log.Warning("DataService", "Error with connection to WebService:");
                Log.Warning("DataService", e.Message);
            }
            return result;
        }

        public async Task<string> UpdateParametersWithScript(string script)
        {
            string url = ScriptUrl + "/" + script;
            string response = await UpdateFromServer(url);
            Log.Warning("DataService", string.Format("UpdateParametersWithScript got {0} from url:{1}", response, url));
            return response;
        }

        public async Task<VoiceMeeter> GetVoiceMeeterFromServer()
        {
            string response = await UpdateFromServer(VoiceMeeterUrl);

            return JsonConvert.DeserializeObject<VoiceMeeter>(response);
            
        }

        private async void UpdateVoiceMeeterLevelsFromServer()
        {
            string response = await UpdateFromServer(LevelsUrl);
            BindingSources.VoiceMeeterLevel = JsonConvert.DeserializeObject<float>(response);
        }
    }
}