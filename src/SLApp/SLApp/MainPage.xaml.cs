using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

namespace SLApp
{
    public partial class MainPage : ContentPage
    {
        private SLClient _client;
        private ISpeech _speech;
        private Uri _uri;

		public MainPage(ISpeech speech, SLClient client, Uri uri)
		{
		    _uri = uri;
		    _speech = speech;
		    _client = client;
			InitializeComponent();
		    this.webView.Source = _uri;
		}

        public async void SpeakItem(object sender, EventArgs args)
	    {
            // let user speak
	        var items = await _speech.SayWords();

            // add items
	        await _client.AddItems(items);

            // refresh web view
            this.webView.Source = _uri;
	    }
    }
}
