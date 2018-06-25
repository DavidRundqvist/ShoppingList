using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;

namespace SLApp.Droid
{
    [Activity(Label = "SLApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISpeech
    {
        private TaskCompletionSource<string[]> _speechTask;




        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(this));
        }

        public Task<string[]> SayWords()
        {
            // create the voice intent
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            // message and modal dialog
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

            // end capturing speech if there is 3 seconds of silence
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            // method to specify other languages to be recognised here if desired
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);


            _speechTask = new TaskCompletionSource<string[]>(); // slight hack. Let's hope the client calls this twice at the same time.
            StartActivityForResult(voiceIntent, 10);
            return _speechTask.Task;
        }


        protected override void OnActivityResult(int requestCode, Result result, Intent data)
        {
            if (requestCode == 10 && result == Result.Ok)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                var matchArray = matches.SelectMany(m => m.Split(' ')).ToArray();
                _speechTask.SetResult(matchArray);
            }
            else
            {
                Console.WriteLine($"Error during speech recognition, code: {result}");
                _speechTask.SetResult(new string[0]);
            }

            base.OnActivityResult(requestCode, result, data);
        }
    }


}

